using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Users;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Pronouns;

public static class PronounsEndpoints
{
    public static IEndpointRouteBuilder MapPronounsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/pronouns", async (AppDbContext db) =>
            Results.Ok(await db.SupportedPronouns
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Label)
                .Select(p => new SupportedPronounsDto
                {
                    Id = p.Id,
                    Label = p.Label,
                    IsBuiltIn = p.IsBuiltIn,
                    SortOrder = p.SortOrder
                })
                .ToListAsync()))
        .RequireAuthorization();

        return app;
    }
}
