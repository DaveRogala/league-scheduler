using LeagueScheduler.Shared.Scheduling;
using Microsoft.Extensions.Options;
using MatchType = LeagueScheduler.Shared.Scheduling.MatchType;

namespace LeagueScheduler.Features.Scheduling
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IScheduleRepository _repo;
        private readonly ScheduleOptions _options;

        public SchedulerService(IScheduleRepository repo, IOptions<ScheduleOptions> opts)
        {
            _repo = repo;
            _options = opts.Value;
        }

        public async Task<ScheduleResultDto> ScheduleAsync(ScheduleRequestDto request)
        {
            var league = request.League;
            var players = request.Players ?? new List<PlayerDto>();

            var eligibleDates = new List<DateTime>();
            DateTime cur = league.StartDate.Date;
            while (cur <= league.EndDate.Date)
            {
                if (league.DaysOfWeek.Contains(cur.DayOfWeek) && !league.NonPlayDates.Any(d => d.Date == cur.Date))
                    eligibleDates.Add(cur);
                cur = cur.AddDays(1);
            }

            int playersPerMatch = league.MatchType == MatchType.Singles ? 2 : 4;
            var slotsPerDate = eligibleDates.ToDictionary(d => d, d => league.Courts * playersPerMatch);
            int totalSlots = slotsPerDate.Values.Sum();

            var playerTargets = players.ToDictionary(p => p.Id, p => (int)Math.Round(p.PreferencePercent * totalSlots));
            var assignedCounts = players.ToDictionary(p => p.Id, p => 0);

            var result = new ScheduleResultDto();

            foreach (var date in eligibleDates)
            {
                int requiredSlots = slotsPerDate[date];
                int matches = requiredSlots / playersPerMatch;
                for (int m = 0; m < matches; m++)
                {
                    var selected = new List<Guid>();
                    for (int pick = 0; pick < playersPerMatch; pick++)
                    {
                        var candidates = players.Where(p => p.Role != PlayerRole.OnCall
                            && !p.UnavailableDates.Any(d => d.Date == date.Date)
                            && !selected.Contains(p.Id))
                            .ToList();

                        if (!candidates.Any())
                        {
                            candidates = players.Where(p => !p.UnavailableDates.Any(d => d.Date == date.Date)
                                && !selected.Contains(p.Id)).ToList();
                        }

                        var ordered = candidates.OrderBy(c =>
                        {
                            int target = playerTargets.ContainsKey(c.Id) ? Math.Max(1, playerTargets[c.Id]) : 1;
                            double ratio = assignedCounts[c.Id] / (double)target;
                            if (c.Nudge == NudgePreference.Above) ratio -= 0.01;
                            if (c.Nudge == NudgePreference.Below) ratio += 0.01;
                            return ratio;
                        }).ThenBy(c => c.PreferencePercent).ToList();

                        var pickPlayer = ordered.FirstOrDefault();
                        if (pickPlayer == null) break;

                        selected.Add(pickPlayer.Id);
                        assignedCounts[pickPlayer.Id]++;
                    }

                    if (selected.Count == playersPerMatch)
                    {
                        result.Matches.Add(new MatchDto { Date = date, Court = m % league.Courts + 1, PlayerIds = selected });
                    }
                    else
                    {
                        result.Conflicts.Add($"Unfilled match on {date:d} court {m + 1}: only assigned {selected.Count} players.");
                    }
                }
            }

            foreach (var kv in assignedCounts) result.AssignedCounts[kv.Key] = kv.Value;
            foreach (var kv in playerTargets) result.TargetCounts[kv.Key] = kv.Value;

            int tolerance = request.FairnessTolerance ?? _options.DefaultFairnessTolerance;
            result.FairnessToleranceUsed = tolerance;

            foreach (var p in players)
            {
                var assigned = assignedCounts.GetValueOrDefault(p.Id);
                var target = playerTargets.GetValueOrDefault(p.Id);
                if (Math.Abs(assigned - target) > tolerance)
                    result.Conflicts.Add($"Player {p.Name} assigned {assigned} vs target {target} exceeds tolerance {tolerance}.");
            }

            await _repo.SaveAsync(result);
            return result;
        }
    }
}
