using LeagueScheduler.Shared.Users;
using Microsoft.AspNetCore.Identity;

namespace LeagueScheduler.Features.Auth.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public Pronouns? Pronouns { get; set; }
        public string? PronounsCustom { get; set; }
        public string? PreferredTimeZone { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedById { get; set; }
    }
}
