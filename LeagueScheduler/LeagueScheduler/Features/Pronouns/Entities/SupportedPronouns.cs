namespace LeagueScheduler.Features.Pronouns.Entities;

public class SupportedPronouns
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public bool IsBuiltIn { get; set; }
    public int SortOrder { get; set; }
}
