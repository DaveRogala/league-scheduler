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
            var season = request.Season;
            var courts = request.Courts;
            var players = request.Players ?? new List<PlayerDto>();

            // --- Phase 1: Build eligible play dates ---
            var eligibleDates = new List<DateTime>();
            DateTime cur = season.StartDate.Date;
            while (cur <= season.EndDate.Date)
            {
                if (season.DaysOfWeek.Contains(cur.DayOfWeek) &&
                    !season.NonPlayDates.Any(d => d.Date == cur.Date))
                    eligibleDates.Add(cur);
                cur = cur.AddDays(1);
            }

            int totalDates = eligibleDates.Count;

            // --- Phase 2: Compute targets ---
            int playersPerMatch = season.MatchType == MatchType.Singles ? 2 : 4;
            int totalSlots = totalDates * courts.Count * playersPerMatch;

            var playerTargets = players.ToDictionary(
                p => p.Id,
                p => (int)Math.Round(p.PreferencePercent * totalSlots));
            var assignedCounts = players.ToDictionary(p => p.Id, p => 0);

            var result = new ScheduleResultDto
            {
                SeasonId = season.Id
            };

            // --- Phase 3: Fill matches with season-paced priority ---
            for (int dateIndex = 0; dateIndex < totalDates; dateIndex++)
            {
                var date = eligibleDates[dateIndex];
                double seasonProgress = (double)(dateIndex + 1) / totalDates;

                foreach (var court in courts)
                {
                    List<Guid> selected = [];

                    for (int pick = 0; pick < playersPerMatch; pick++)
                    {
                        // OnCall players are never scheduled — they exist only as a
                        // contact list for manual substitutions outside the algorithm.
                        var candidates = players
                            .Where(p => p.Role != PlayerRole.OnCall
                                && !p.UnavailableDates.Any(d => d.Date == date.Date)
                                && !selected.Contains(p.Id))
                            .ToList();

                        var ordered = candidates
                            .OrderBy(c =>
                            {
                                int target = Math.Max(1, playerTargets.GetValueOrDefault(c.Id));
                                double expectedByNow = target * seasonProgress;
                                double ratio = assignedCounts[c.Id] / Math.Max(0.5, expectedByNow);

                                if (c.Nudge == NudgePreference.Above) ratio -= 0.05;
                                if (c.Nudge == NudgePreference.Below) ratio += 0.05;

                                return ratio;
                            })
                            .ThenBy(c => c.PreferencePercent)
                            .ToList();

                        var picked = ordered.FirstOrDefault();
                        if (picked == null) break;

                        selected.Add(picked.Id);
                        assignedCounts[picked.Id]++;
                    }

                    if (selected.Count == playersPerMatch)
                    {
                        result.Matches.Add(new MatchDto
                        {
                            Date = date,
                            CourtId = court.Id,
                            CourtName = court.Name,
                            PlayerIds = selected
                        });
                    }
                    else
                    {
                        result.Conflicts.Add(
                            $"Unfilled match on {date:d} court \"{court.Name}\": only assigned {selected.Count}/{playersPerMatch} players.");
                    }
                }
            }

            // --- Phase 4: Populate result summary ---
            foreach (var kv in assignedCounts) result.AssignedCounts[kv.Key] = kv.Value;
            foreach (var kv in playerTargets) result.TargetCounts[kv.Key] = kv.Value;

            // --- Phase 5: Fairness validation ---
            int tolerance = request.FairnessTolerance ?? _options.DefaultFairnessTolerance;
            result.FairnessToleranceUsed = tolerance;

            foreach (var p in players)
            {
                int assigned = assignedCounts.GetValueOrDefault(p.Id);
                int target = playerTargets.GetValueOrDefault(p.Id);
                if (Math.Abs(assigned - target) > tolerance)
                {
                    result.Conflicts.Add(
                        $"Player {p.Name} assigned {assigned} vs target {target} exceeds tolerance {tolerance}.");
                }
            }

            await _repo.SaveAsync(result);
            return result;
        }
    }
}
