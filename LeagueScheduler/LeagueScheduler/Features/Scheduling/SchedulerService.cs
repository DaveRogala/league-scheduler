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

            // --- Phase 1: Build eligible play dates ---
            // Walk the full date range, keeping only days that match the league's
            // scheduled days-of-week and aren't blocked as non-play dates (holidays, etc.).
            var eligibleDates = new List<DateTime>();
            DateTime cur = league.StartDate.Date;
            while (cur <= league.EndDate.Date)
            {
                if (league.DaysOfWeek.Contains(cur.DayOfWeek) &&
                    !league.NonPlayDates.Any(d => d.Date == cur.Date))
                    eligibleDates.Add(cur);
                cur = cur.AddDays(1);
            }

            int totalDates = eligibleDates.Count;

            // --- Phase 2: Compute targets ---
            // Each play date fills (courts × playersPerMatch) slots.
            // A player's target is how many of those total slots they should occupy,
            // derived from their stated preference percentage.
            int playersPerMatch = league.MatchType == MatchType.Singles ? 2 : 4;
            int totalSlots = totalDates * league.Courts * playersPerMatch;

            var playerTargets = players.ToDictionary(
                p => p.Id,
                p => (int)Math.Round(p.PreferencePercent * totalSlots));
            var assignedCounts = players.ToDictionary(p => p.Id, p => 0);

            var result = new ScheduleResultDto();

            // --- Phase 3: Fill matches with season-paced priority ---
            // For each play date we compute a "season progress" fraction (0→1 over the season).
            // Each candidate player is scored by a pacing ratio:
            //
            //   ratio = assignedSoFar / expectedByNow
            //   expectedByNow = target × seasonProgress
            //
            // A player below their expected pace (low ratio) gets priority over one who is
            // already ahead of pace (high ratio). This distributes assignments proportionally
            // throughout the season rather than front-loading high-percentage players early.
            for (int dateIndex = 0; dateIndex < totalDates; dateIndex++)
            {
                var date = eligibleDates[dateIndex];

                // seasonProgress uses (dateIndex + 1) so the very first date isn't 0,
                // which would make expectedByNow = 0 for everyone and lose discrimination.
                double seasonProgress = (double)(dateIndex + 1) / totalDates;

                for (int court = 0; court < league.Courts; court++)
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

                        // Score each candidate by their pacing ratio.
                        // Math.Max(0.5, expectedByNow) floors the denominator so we don't
                        // divide by near-zero at the very start of the season, and avoids
                        // wildly inflating the ratio for players with tiny targets.
                        var ordered = candidates
                            .OrderBy(c =>
                            {
                                int target = Math.Max(1, playerTargets.GetValueOrDefault(c.Id));
                                double expectedByNow = target * seasonProgress;
                                double ratio = assignedCounts[c.Id] / Math.Max(0.5, expectedByNow);

                                // Nudge shifts the ratio slightly to honor the player's stated
                                // preference without overriding the pacing logic.
                                if (c.Nudge == NudgePreference.Above) ratio -= 0.05;
                                if (c.Nudge == NudgePreference.Below) ratio += 0.05;

                                return ratio;
                            })
                            .ThenBy(c => c.PreferencePercent)
                            .ToList();

                        var pick_player = ordered.FirstOrDefault();
                        if (pick_player == null) break;

                        selected.Add(pick_player.Id);
                        assignedCounts[pick_player.Id]++;
                    }

                    if (selected.Count == playersPerMatch)
                    {
                        result.Matches.Add(new MatchDto
                        {
                            Date = date,
                            Court = court + 1,
                            PlayerIds = selected
                        });
                    }
                    else
                    {
                        result.Conflicts.Add(
                            $"Unfilled match on {date:d} court {court + 1}: only assigned {selected.Count}/{playersPerMatch} players.");
                    }
                }
            }

            // --- Phase 4: Populate result summary ---
            foreach (var kv in assignedCounts) result.AssignedCounts[kv.Key] = kv.Value;
            foreach (var kv in playerTargets) result.TargetCounts[kv.Key] = kv.Value;

            // --- Phase 5: Fairness validation ---
            // Flag any player whose final assigned count deviates from their target
            // by more than the configured tolerance (slots, not percentage).
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
