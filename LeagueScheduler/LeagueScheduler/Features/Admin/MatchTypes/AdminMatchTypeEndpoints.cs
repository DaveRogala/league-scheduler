using LeagueScheduler.Features.MatchTypes;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.MatchTypes;
using Microsoft.EntityFrameworkCore;
using MatchTypeEntity = LeagueScheduler.Features.MatchTypes.Entities.MatchType;

namespace LeagueScheduler.Features.Admin.MatchTypes
{
    public static class AdminMatchTypeEndpoints
    {
        public static IEndpointRouteBuilder MapAdminMatchTypeEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/admin/match-types").RequireAuthorization();

            g.MapPost("", async (MatchTypeDto dto, AppDbContext db) =>
            {
                if (await db.MatchTypes.AnyAsync(m => m.Name == dto.Name.Trim()))
                    return Results.Conflict("A match type with that name already exists.");

                var matchType = new MatchTypeEntity
                {
                    Name = dto.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                    MinPlayersPerCourt = dto.MinPlayersPerCourt,
                    MaxPlayersPerCourt = dto.MaxPlayersPerCourt,
                    MustHaveEvenPlayers = dto.MustHaveEvenPlayers,
                    IsBuiltIn = false,
                    SortOrder = dto.SortOrder
                };
                db.MatchTypes.Add(matchType);
                await db.SaveChangesAsync();
                return Results.Created($"/api/match-types/{matchType.Id}", MatchTypeEndpoints.ToDto(matchType));
            });

            g.MapPut("{id:guid}", async (Guid id, MatchTypeDto dto, AppDbContext db) =>
            {
                var matchType = await db.MatchTypes.FindAsync(id);
                if (matchType is null) return Results.NotFound();

                if (await db.MatchTypes.AnyAsync(m => m.Name == dto.Name.Trim() && m.Id != id))
                    return Results.Conflict("A match type with that name already exists.");

                // Built-in types allow description and sort order updates but not structural changes.
                if (!matchType.IsBuiltIn)
                {
                    matchType.MinPlayersPerCourt = dto.MinPlayersPerCourt;
                    matchType.MaxPlayersPerCourt = dto.MaxPlayersPerCourt;
                    matchType.MustHaveEvenPlayers = dto.MustHaveEvenPlayers;
                }

                matchType.Name = dto.Name.Trim();
                matchType.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
                matchType.SortOrder = dto.SortOrder;

                await db.SaveChangesAsync();
                return Results.Ok(MatchTypeEndpoints.ToDto(matchType));
            });

            g.MapDelete("{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var matchType = await db.MatchTypes.FindAsync(id);
                if (matchType is null) return Results.NotFound();
                if (matchType.IsBuiltIn) return Results.Conflict("Built-in match types cannot be deleted.");

                db.MatchTypes.Remove(matchType);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }
    }
}
