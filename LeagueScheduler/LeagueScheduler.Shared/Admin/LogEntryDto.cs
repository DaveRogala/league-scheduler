namespace LeagueScheduler.Shared.Admin
{
    public record LogEntryDto
    {
        public Guid Id { get; init; }
        public DateTimeOffset Timestamp { get; init; }
        public string Level { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string? Exception { get; init; }
        // Raw JSON of the structured event properties logged by Serilog.
        public string? Properties { get; init; }
    }
}
