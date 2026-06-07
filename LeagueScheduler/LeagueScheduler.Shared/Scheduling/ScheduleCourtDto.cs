namespace LeagueScheduler.Shared.Scheduling
{
    public record ScheduleCourtDto
    {
        public required Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
