using LeagueScheduler.Features.Pronouns.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Users;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Admin.Pronouns;

public static class AdminPronounsEndpoints
{
    public static IEndpointRouteBuilder MapAdminPronounsEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/admin/pronouns").RequireAuthorization();

        g.MapPost("", async (SupportedPronounsDto dto, AppDbContext db) =>
        {
            var next = await db.SupportedPronouns.MaxAsync(p => (int?)p.SortOrder) ?? 0;
            var pronoun = new SupportedPronouns
            {
                Label = dto.Label.Trim(),
                IsBuiltIn = false,
                SortOrder = next + 1
            };
            db.SupportedPronouns.Add(pronoun);
            await db.SaveChangesAsync();
            return Results.Created($"/api/admin/pronouns/{pronoun.Id}", new SupportedPronounsDto
            {
                Id = pronoun.Id,
                Label = pronoun.Label,
                IsBuiltIn = pronoun.IsBuiltIn,
                SortOrder = pronoun.SortOrder
            });
        });

        g.MapDelete("{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var pronoun = await db.SupportedPronouns.FindAsync(id);
            if (pronoun is null) return Results.NotFound();
            if (pronoun.IsBuiltIn)
                return Results.Conflict("Built-in pronoun sets cannot be removed.");

            db.SupportedPronouns.Remove(pronoun);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return app;
    }
}
