namespace LeagueScheduler.Shared.Scheduling
{
    public record ScheduleCourtDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
    }
}
