namespace LeagueScheduler.Shared.Admin
{
    public record SupportedTimeZoneDto
    {
        public string Id { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public double UtcOffsetHours { get; init; }
        public bool IsEnabled { get; init; }
    }
}
