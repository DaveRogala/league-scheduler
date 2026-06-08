using LeagueScheduler.Features.Admin.TimeZones.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Admin;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Admin.TimeZones
{
    public static class TimeZoneEndpoints
    {
        public static IEndpointRouteBuilder MapTimeZoneEndpoints(this IEndpointRouteBuilder app)
        {
            // Public: enabled time zones for profile dropdowns
            app.MapGet("/api/timezones", async (AppDbContext db) =>
                Results.Ok(await db.SupportedTimeZones
                    .Where(t => t.IsEnabled)
                    .OrderBy(t => t.UtcOffsetHours)
                    .ThenBy(t => t.DisplayName)
                    .Select(t => ToDto(t))
                    .ToListAsync()))
                .RequireAuthorization();

            // Admin: all time zones (enabled and disabled)
            var admin = app.MapGroup("/api/admin/timezones").RequireAuthorization();

            admin.MapGet("", async (AppDbContext db) =>
                Results.Ok(await db.SupportedTimeZones
                    .OrderBy(t => t.UtcOffsetHours)
                    .ThenBy(t => t.DisplayName)
                    .Select(t => ToDto(t))
                    .ToListAsync()));

            admin.MapPut("{**id}", async (string id, SupportedTimeZoneDto dto, AppDbContext db) =>
            {
                var tz = await db.SupportedTimeZones.FindAsync(id);
                if (tz is null) return Results.NotFound();
                tz.IsEnabled = dto.IsEnabled;
                await db.SaveChangesAsync();
                return Results.Ok(ToDto(tz));
            });

            return app;
        }

        private static SupportedTimeZoneDto ToDto(SupportedTimeZone t) => new()
        {
            Id = t.Id,
            DisplayName = t.DisplayName,
            UtcOffsetHours = t.UtcOffsetHours,
            IsEnabled = t.IsEnabled
        };
    }
}
