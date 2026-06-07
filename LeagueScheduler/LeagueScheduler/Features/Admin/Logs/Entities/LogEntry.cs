namespace LeagueScheduler.Features.Admin.Logs.Entities
{
    public class LogEntry
    {
        public Guid Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        // Structured properties serialized as JSON by the Serilog PostgreSQL sink.
        public string? Properties { get; set; }
    }
}
