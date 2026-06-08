using LeagueScheduler.Features.Auth;
using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Shared.Admin.Users;
using LeagueScheduler.Shared.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LeagueScheduler.Features.Admin.Users;

public static class AdminUserEndpoints
{
    public static IEndpointRouteBuilder MapAdminUserEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/admin/users").RequireAuthorization();

        g.MapGet("", async (
            UserManager<AppUser> userManager,
            string? search,
            int page = 1,
            int pageSize = 20) =>
        {
            var query = userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(u =>
                    u.Email!.ToLower().Contains(term) ||
                    u.DisplayName.ToLower().Contains(term));
            }

            var total = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Email)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = new List<AdminUserDto>(users.Count);
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var isDisabled = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;
                dtos.Add(new AdminUserDto(user.Id, user.Email!, user.DisplayName, isDisabled, roles.ToList()));
            }

            return Results.Ok(new AdminUserListDto(dtos, total, page, pageSize));
        });

        g.MapPost("{id:guid}/reset-password", async (
            Guid id,
            AdminResetPasswordDto dto,
            UserManager<AppUser> userManager,
            ClaimsPrincipal currentUser,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminUserEndpoints));

            if (dto.NewPassword != dto.ConfirmPassword)
                return Results.BadRequest("Passwords do not match.");

            var user = await userManager.FindByIdAsync(id.ToString());
            if (user is null) return Results.NotFound();

            var remove = await userManager.RemovePasswordAsync(user);
            if (!remove.Succeeded)
                return Results.Problem("Failed to remove existing password.");

            var add = await userManager.AddPasswordAsync(user, dto.NewPassword);
            if (!add.Succeeded)
                return Results.ValidationProblem(add.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));

            var adminId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            logger.LogInformation("Admin {AdminId} reset password for user {UserId}", adminId, id);
            return Results.NoContent();
        });

        g.MapPost("{id:guid}/disable", async (
            Guid id,
            UserManager<AppUser> userManager,
            ClaimsPrincipal currentUser,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminUserEndpoints));

            var user = await userManager.FindByIdAsync(id.ToString());
            if (user is null) return Results.NotFound();

            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            var adminId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            logger.LogInformation("Admin {AdminId} disabled account for user {UserId}", adminId, id);
            return Results.NoContent();
        });

        g.MapPost("{id:guid}/enable", async (
            Guid id,
            UserManager<AppUser> userManager,
            ClaimsPrincipal currentUser,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminUserEndpoints));

            var user = await userManager.FindByIdAsync(id.ToString());
            if (user is null) return Results.NotFound();

            await userManager.SetLockoutEndDateAsync(user, null);

            var adminId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            logger.LogInformation("Admin {AdminId} enabled account for user {UserId}", adminId, id);
            return Results.NoContent();
        });

        g.MapPost("{id:guid}/impersonate", async (
            Guid id,
            UserManager<AppUser> userManager,
            JwtService jwt,
            ClaimsPrincipal currentUser,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminUserEndpoints));

            var adminIdStr = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(adminIdStr, out var adminId))
                return Results.Unauthorized();

            if (adminId == id)
                return Results.BadRequest("You cannot impersonate yourself.");

            var targetUser = await userManager.FindByIdAsync(id.ToString());
            if (targetUser is null) return Results.NotFound();

            var isDisabled = targetUser.LockoutEnd.HasValue && targetUser.LockoutEnd.Value > DateTimeOffset.UtcNow;
            if (isDisabled)
                return Results.BadRequest("Cannot impersonate a disabled account.");

            var token = jwt.GenerateImpersonationToken(targetUser, adminId);

            logger.LogWarning(
                "Admin {AdminId} started impersonation of user {TargetUserId} ({TargetEmail})",
                adminId, targetUser.Id, targetUser.Email);

            return Results.Ok(new AuthResultDto
            {
                Success = true,
                Token = token,
                Email = targetUser.Email,
                DisplayName = targetUser.DisplayName
            });
        });

        return app;
    }
}
