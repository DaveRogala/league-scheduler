using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Shared.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LeagueScheduler.Features.Users
{
    public static class UserProfileEndpoints
    {
        public static IEndpointRouteBuilder MapUserProfileEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/profile").RequireAuthorization();

            g.MapGet("", async (ClaimsPrincipal principal, UserManager<AppUser> users) =>
            {
                var user = await users.GetUserAsync(principal);
                return user is null ? Results.Unauthorized() : Results.Ok(ToDto(user));
            });

            g.MapPut("", async (UserProfileDto dto, ClaimsPrincipal principal, UserManager<AppUser> users) =>
            {
                var user = await users.GetUserAsync(principal);
                if (user is null) return Results.Unauthorized();

                user.FirstName = dto.FirstName.Trim();
                user.MiddleName = string.IsNullOrWhiteSpace(dto.MiddleName) ? null : dto.MiddleName.Trim();
                user.LastName = dto.LastName.Trim();
                user.DisplayName = dto.DisplayName.Trim();
                user.Pronouns = dto.Pronouns;
                user.PronounsCustom = dto.Pronouns == Pronouns.Other
                    ? dto.PronounsCustom?.Trim()
                    : null;
                user.PreferredTimeZone = string.IsNullOrWhiteSpace(dto.PreferredTimeZone)
                    ? null
                    : dto.PreferredTimeZone.Trim();

                var result = await users.UpdateAsync(user);
                return result.Succeeded
                    ? Results.Ok(ToDto(user))
                    : Results.BadRequest(result.Errors.Select(e => e.Description));
            });

            return app;
        }

        private static UserProfileDto ToDto(AppUser user) => new()
        {
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            DisplayName = user.DisplayName,
            Pronouns = user.Pronouns,
            PronounsCustom = user.PronounsCustom,
            PreferredTimeZone = user.PreferredTimeZone
        };
    }
}
