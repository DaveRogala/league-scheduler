using LeagueScheduler.Features.Leagues.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Leagues;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Leagues
{
    public static class LeagueEndpoints
    {
        public static IEndpointRouteBuilder MapLeagueEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/leagues").RequireAuthorization();

            g.MapGet("", async (AppDbContext db) =>
                Results.Ok(await db.Leagues
                    .OrderBy(l => l.Name)
                    .Select(l => ToDto(l))
                    .ToListAsync()));

            g.MapGet("{id:guid}", async (Guid id, AppDbContext db) =>
                await db.Leagues.FindAsync(id) is League l
                    ? Results.Ok(ToDto(l))
                    : Results.NotFound());

            g.MapPost("", async (LeagueDto dto, AppDbContext db) =>
            {
                var league = new League
                {
                    Name = dto.Name.Trim(),
                    Mode = dto.Mode,
                    MatchType = dto.MatchType,
                    RequireApprovalToJoin = dto.RequireApprovalToJoin
                };
                db.Leagues.Add(league);
                await db.SaveChangesAsync();
                return Results.Created($"/api/leagues/{league.Id}", ToDto(league));
            });

            g.MapPut("{id:guid}", async (Guid id, LeagueDto dto, AppDbContext db) =>
            {
                var league = await db.Leagues.FindAsync(id);
                if (league is null) return Results.NotFound();
                league.Name = dto.Name.Trim();
                league.Mode = dto.Mode;
                league.MatchType = dto.MatchType;
                league.RequireApprovalToJoin = dto.RequireApprovalToJoin;
                await db.SaveChangesAsync();
                return Results.Ok(ToDto(league));
            });

            g.MapDelete("{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var league = await db.Leagues.FindAsync(id);
                if (league is null) return Results.NotFound();
                db.Leagues.Remove(league);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }

        private static LeagueDto ToDto(League l) => new()
        {
            Id = l.Id,
            Name = l.Name,
            Mode = l.Mode,
            MatchType = l.MatchType,
            RequireApprovalToJoin = l.RequireApprovalToJoin
        };
    }
}
