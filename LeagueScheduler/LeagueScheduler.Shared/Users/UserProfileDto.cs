namespace LeagueScheduler.Shared.Users
{
    public record UserProfileDto
    {
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string? MiddleName { get; init; }
        public string LastName { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public Pronouns? Pronouns { get; init; }
        public string? PronounsCustom { get; init; }
        public string? PreferredTimeZone { get; init; }
    }
}
