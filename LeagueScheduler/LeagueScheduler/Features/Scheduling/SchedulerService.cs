using LeagueScheduler.Shared.Scheduling;
using Microsoft.Extensions.Options;

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

        /// <summary>
        /// Generates a schedule for a season in three stages:
        /// 1. Date expansion — walks StartDate..EndDate, keeping days that match DaysOfWeek and
        ///    are not in NonPlayDates.
        /// 2. Match filling — for each eligible date × court slot, greedily picks the players
        ///    whose assigned/expected ratio lags most behind the season's current progress.
        ///    NudgePreference nudges a player's effective ratio slightly up or down. OnCall
        ///    players are never auto-scheduled; AsNeeded players fill remaining gaps.
        /// 3. Fairness validation — flags any player whose final assignment count differs from
        ///    their proportional target by more than FairnessTolerance.
        /// </summary>
        public async Task<ScheduleResultDto> ScheduleAsync(ScheduleRequestDto request)
        {
            var season = request.Season;
            var courts = request.Courts;
            var players = request.Players ?? [];

            int playersPerMatch = season.MatchType?.MaxPlayersPerCourt ?? 4;
            var eligibleDates = BuildEligibleDates(season);
            int totalSlots = eligibleDates.Count * courts.Count * playersPerMatch;

            var playerTargets = ComputeTargets(players, totalSlots);
            var assignedCounts = players.ToDictionary(p => p.Id, p => 0);

            var result = new ScheduleResultDto { SeasonId = season.Id };
            FillSchedule(eligibleDates, courts, players, playerTargets, assignedCounts, result, playersPerMatch);

            foreach (var kv in assignedCounts) result.AssignedCounts[kv.Key] = kv.Value;
            foreach (var kv in playerTargets) result.TargetCounts[kv.Key] = kv.Value;

            int tolerance = request.FairnessTolerance ?? _options.DefaultFairnessTolerance;
            result.FairnessToleranceUsed = tolerance;
            result.Conflicts.AddRange(ValidateFairness(players, assignedCounts, playerTargets, tolerance));

            return await _repo.SaveAsync(result);
        }

        // Expands the season's date range into the list of dates on which play is allowed.
        private static List<DateTime> BuildEligibleDates(SeasonDto season)
        {
            var dates = new List<DateTime>();
            for (var d = season.StartDate.Date; d <= season.EndDate.Date; d = d.AddDays(1))
            {
                if (season.DaysOfWeek.Contains(d.DayOfWeek) &&
                    !season.NonPlayDates.Any(n => n.Date == d.Date))
                    dates.Add(d);
            }
            return dates;
        }

        // Each player's target is their PreferencePercent share of the total available slots.
        private static Dictionary<Guid, int> ComputeTargets(List<PlayerDto> players, int totalSlots) =>
            players.ToDictionary(
                p => p.Id,
                p => (int)Math.Round(p.PreferencePercent * totalSlots));

        // Iterates every date × court combination and fills each slot with the best available players.
        private static void FillSchedule(
            List<DateTime> eligibleDates,
            List<ScheduleCourtDto> courts,
            List<PlayerDto> players,
            Dictionary<Guid, int> playerTargets,
            Dictionary<Guid, int> assignedCounts,
            ScheduleResultDto result,
            int playersPerMatch)
        {
            int totalDates = eligibleDates.Count;
            for (int dateIndex = 0; dateIndex < totalDates; dateIndex++)
            {
                var date = eligibleDates[dateIndex];
                double seasonProgress = (double)(dateIndex + 1) / totalDates;

                foreach (var court in courts)
                {
                    var selected = SelectPlayersForCourt(
                        players, playerTargets, assignedCounts, date, seasonProgress, playersPerMatch);

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
        }

        // Greedily picks up to playersPerMatch players for a single court/date slot.
        private static List<Guid> SelectPlayersForCourt(
            List<PlayerDto> players,
            Dictionary<Guid, int> playerTargets,
            Dictionary<Guid, int> assignedCounts,
            DateTime date,
            double seasonProgress,
            int playersPerMatch)
        {
            var selected = new List<Guid>();

            for (int pick = 0; pick < playersPerMatch; pick++)
            {
                // OnCall players are never scheduled — they exist only as a contact list for
                // manual substitutions outside the algorithm.
                var picked = players
                    .Where(p => p.Role != PlayerRole.OnCall
                        && !p.UnavailableDates.Any(d => d.Date == date.Date)
                        && !selected.Contains(p.Id))
                    .OrderBy(p => ComputePriority(p, assignedCounts, playerTargets, seasonProgress))
                    .ThenBy(p => p.PreferencePercent)
                    .FirstOrDefault();

                if (picked == null) break;

                selected.Add(picked.Id);
                assignedCounts[picked.Id]++;
            }

            return selected;
        }

        // Lower ratio = higher priority. NudgePreference shifts the ratio by ±0.05 to break ties
        // in favour of players who prefer to play slightly more or less than their exact target.
        private static double ComputePriority(
            PlayerDto player,
            Dictionary<Guid, int> assignedCounts,
            Dictionary<Guid, int> playerTargets,
            double seasonProgress)
        {
            int target = Math.Max(1, playerTargets.GetValueOrDefault(player.Id));
            double expectedByNow = target * seasonProgress;
            double ratio = assignedCounts[player.Id] / Math.Max(0.5, expectedByNow);

            if (player.Nudge == NudgePreference.Above) ratio -= 0.05;
            if (player.Nudge == NudgePreference.Below) ratio += 0.05;

            return ratio;
        }

        // Yields a conflict message for each player whose final count strays beyond tolerance.
        private static IEnumerable<string> ValidateFairness(
            List<PlayerDto> players,
            Dictionary<Guid, int> assignedCounts,
            Dictionary<Guid, int> playerTargets,
            int tolerance)
        {
            foreach (var p in players)
            {
                int assigned = assignedCounts.GetValueOrDefault(p.Id);
                int target = playerTargets.GetValueOrDefault(p.Id);
                if (Math.Abs(assigned - target) > tolerance)
                    yield return $"Player {p.Name} assigned {assigned} vs target {target} exceeds tolerance {tolerance}.";
            }
        }
    }
}
