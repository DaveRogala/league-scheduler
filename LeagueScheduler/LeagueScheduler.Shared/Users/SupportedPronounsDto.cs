namespace LeagueScheduler.Shared.Users;

public record SupportedPronounsDto
{
    public Guid Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public bool IsBuiltIn { get; init; }
    public int SortOrder { get; init; }
}
