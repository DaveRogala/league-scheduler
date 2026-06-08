namespace LeagueScheduler.Shared.Common
{
    public record CountryDto
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public bool IsBuiltIn { get; init; }
        public int SortOrder { get; init; }
        public string Region1Name { get; init; } = "State/Province";
        public bool Region1UseList { get; init; }
        public bool DisplayRegion2 { get; init; }
        public string? Region2Name { get; init; }
    }
}
