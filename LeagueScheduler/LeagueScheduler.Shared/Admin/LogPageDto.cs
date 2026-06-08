namespace LeagueScheduler.Shared.Admin
{
    public record LogPageDto
    {
        public List<LogEntryDto> Items { get; init; } = new();
        public int TotalCount { get; init; }
    }
}
