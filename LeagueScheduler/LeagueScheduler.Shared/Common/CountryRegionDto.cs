namespace LeagueScheduler.Shared.Common
{
    public record CountryRegionDto
    {
        public Guid Id { get; init; }
        public Guid CountryId { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public int SortOrder { get; init; }
    }
}
