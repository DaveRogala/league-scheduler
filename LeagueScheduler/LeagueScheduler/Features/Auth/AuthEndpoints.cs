using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Shared.Auth;
using Microsoft.AspNetCore.Identity;

namespace LeagueScheduler.Features.Auth
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/api/auth/register", async (
                RegisterRequestDto dto,
                UserManager<AppUser> userManager,
                JwtService jwt) =>
            {
                if (dto.Password != dto.ConfirmPassword)
                    return Results.Ok(new AuthResultDto { Errors = ["Passwords do not match."] });

                var user = new AppUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    DisplayName = dto.DisplayName
                };

                var result = await userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                    return Results.Ok(new AuthResultDto
                    {
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });

                var token = jwt.GenerateToken(user);
                return Results.Ok(new AuthResultDto
                {
                    Success = true,
                    Token = token,
                    Email = user.Email,
                    DisplayName = user.DisplayName
                });
            });

            app.MapPost("/api/auth/login", async (
                LoginRequestDto dto,
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                JwtService jwt) =>
            {
                var user = await userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                    return Results.Ok(new AuthResultDto { Errors = ["Invalid email or password."] });

                var check = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
                if (!check.Succeeded)
                    return Results.Ok(new AuthResultDto { Errors = ["Invalid email or password."] });

                var token = jwt.GenerateToken(user);
                return Results.Ok(new AuthResultDto
                {
                    Success = true,
                    Token = token,
                    Email = user.Email,
                    DisplayName = user.DisplayName
                });
            });
        }
    }
}
