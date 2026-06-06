using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueScheduler.Shared.Models;
using MatchType = LeagueScheduler.Shared.Models.MatchType;

namespace LeagueScheduler.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IScheduleRepository _repo;
        private readonly ScheduleOptions _options;

        public SchedulerService(IScheduleRepository repo, Microsoft.Extensions.Options.IOptions<ScheduleOptions> opts)
        {
            _repo = repo;
            _options = opts.Value;
        }

        public async Task<ScheduleResultDto> ScheduleAsync(ScheduleRequestDto request)
        {
            var league = request.League;
            var players = request.Players ?? new List<PlayerDto>();

            // Expand eligible dates
            var eligibleDates = new List<DateTime>();
            DateTime cur = league.StartDate.Date;
            while (cur <= league.EndDate.Date)
            {
                if (league.DaysOfWeek.Contains(cur.DayOfWeek) && !league.NonPlayDates.Any(d => d.Date == cur.Date))
                    eligibleDates.Add(cur);
                cur = cur.AddDays(1);
            }

            int playersPerMatch = league.MatchType == MatchType.Singles ? 2 : 4;
            // slots per date = courts * playersPerMatch
            var slotsPerDate = eligibleDates.ToDictionary(d => d, d => league.Courts * playersPerMatch);
            int totalSlots = slotsPerDate.Values.Sum();

            // compute target slots per player (round to nearest int)
            var playerTargets = players.ToDictionary(p => p.Id, p => (int)Math.Round(p.PreferencePercent * totalSlots));
            var assignedCounts = players.ToDictionary(p => p.Id, p => 0);

            var result = new ScheduleResultDto();

            // Build per-date assignments
            foreach (var date in eligibleDates)
            {
                int requiredSlots = slotsPerDate[date];
                int matches = requiredSlots / playersPerMatch;
                for (int m = 0; m < matches; m++)
                {
                    var selected = new List<Guid>();
                    // Select players for this match
                    // build match with players via initializer later
                    for (int pick = 0; pick < playersPerMatch; pick++)
                    {
                        // Candidate players: available that date, not already selected for this match, not OnCall
                        var candidates = players.Where(p => p.Role != PlayerRole.OnCall
                            && !p.UnavailableDates.Any(d => d.Date == date.Date)
                            && !selected.Contains(p.Id))
                            .ToList();

                        if (!candidates.Any())
                        {
                            // If no candidates, try OnCall
                            candidates = players.Where(p => !p.UnavailableDates.Any(d => d.Date == date.Date)
                                && !selected.Contains(p.Id)).ToList();
                        }

                        // Order by (assigned/target) ascending — those most below target get priority
                        var ordered = candidates.OrderBy(c =>
                        {
                            int target = playerTargets.ContainsKey(c.Id) ? Math.Max(1, playerTargets[c.Id]) : 1;
                            double ratio = assignedCounts[c.Id] / (double)target;
                            // Nudge preference involvement: Above gets slight bonus (lower ratio)
                            if (c.Nudge == NudgePreference.Above) ratio -= 0.01;
                            if (c.Nudge == NudgePreference.Below) ratio += 0.01;
                            return ratio;
                        }).ThenBy(c => c.PreferencePercent).ToList();

                        var pickPlayer = ordered.FirstOrDefault();
                        if (pickPlayer == null)
                        {
                            // no available player — leave slot empty
                            break;
                        }

                        selected.Add(pickPlayer.Id);
                        assignedCounts[pickPlayer.Id]++;
                    }

                    if (selected.Count == playersPerMatch)
                    {
                        var match = new MatchDto { Date = date, Court = m % league.Courts + 1, PlayerIds = selected };
                        result.Matches.Add(match);
                    }
                    else
                    {
                        result.Conflicts.Add($"Unfilled match on {date:d} court {m + 1}: only assigned {selected.Count} players.");
                    }
                }
            }

            // Build AssignedCounts and TargetCounts in result
            foreach (var kv in assignedCounts)
            {
                result.AssignedCounts[kv.Key] = kv.Value;
            }
            foreach (var kv in playerTargets)
            {
                result.TargetCounts[kv.Key] = kv.Value;
            }

            // Determine fairness tolerance used (request override -> server default)
            int tolerance = request.FairnessTolerance ?? _options.DefaultFairnessTolerance;
            result.FairnessToleranceUsed = tolerance;

            // Validate fairness: flag players outside tolerance
            foreach (var p in players)
            {
                var assigned = assignedCounts.ContainsKey(p.Id) ? assignedCounts[p.Id] : 0;
                var target = playerTargets.ContainsKey(p.Id) ? playerTargets[p.Id] : 0;
                if (Math.Abs(assigned - target) > tolerance)
                {
                    result.Conflicts.Add($"Player {p.Name} assigned {assigned} vs target {target} exceeds tolerance {tolerance}.");
                }
            }

            // Persist result
            await _repo.SaveAsync(result);

            return result;
        }
    }
}
