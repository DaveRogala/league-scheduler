using Microsoft.AspNetCore.Identity;

namespace LeagueScheduler.Features.Auth.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; } = string.Empty;
    }
}
