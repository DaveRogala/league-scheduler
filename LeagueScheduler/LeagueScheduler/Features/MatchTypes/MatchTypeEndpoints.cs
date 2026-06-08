using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.MatchTypes;
using Microsoft.EntityFrameworkCore;
using MatchTypeEntity = LeagueScheduler.Features.MatchTypes.Entities.MatchType;

namespace LeagueScheduler.Features.MatchTypes
{
    public static class MatchTypeEndpoints
    {
        public static IEndpointRouteBuilder MapMatchTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/match-types", async (AppDbContext db) =>
                Results.Ok(await db.MatchTypes
                    .OrderBy(m => m.SortOrder)
                    .ThenBy(m => m.Name)
                    .Select(m => ToDto(m))
                    .ToListAsync()))
                .RequireAuthorization();

            return app;
        }

        internal static MatchTypeDto ToDto(MatchTypeEntity m) => new()
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            MinPlayersPerCourt = m.MinPlayersPerCourt,
            MaxPlayersPerCourt = m.MaxPlayersPerCourt,
            MustHaveEvenPlayers = m.MustHaveEvenPlayers,
            IsBuiltIn = m.IsBuiltIn,
            SortOrder = m.SortOrder
        };
    }
}
