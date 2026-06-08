using LeagueScheduler.Shared.MatchTypes;

namespace LeagueScheduler.Shared.Scheduling
{
    public record SeasonDto
    {
        public required Guid Id { get; init; }
        public Guid LeagueId { get; init; }
        public string Name { get; init; } = string.Empty;
        public MatchTypeDto? MatchType { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public List<DayOfWeek> DaysOfWeek { get; init; } = new();
        public List<DateTime> NonPlayDates { get; init; } = new();
        public List<PrePlannedEventDto> PrePlannedEvents { get; init; } = new();
    }
}
