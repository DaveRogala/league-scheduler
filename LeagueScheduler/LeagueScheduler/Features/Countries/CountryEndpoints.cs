using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Countries
{
    public static class CountryEndpoints
    {
        public static IEndpointRouteBuilder MapCountryEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/countries").RequireAuthorization();

            g.MapGet("", async (AppDbContext db) =>
                Results.Ok(await db.Countries
                    .OrderBy(c => c.SortOrder)
                    .ThenBy(c => c.Name)
                    .Select(c => ToDto(c))
                    .ToListAsync()));

            g.MapGet("{id:guid}/regions", async (Guid id, AppDbContext db) =>
                Results.Ok(await db.CountryRegions
                    .Where(r => r.CountryId == id)
                    .OrderBy(r => r.SortOrder)
                    .ThenBy(r => r.Name)
                    .Select(r => new CountryRegionDto
                    {
                        Id = r.Id,
                        CountryId = r.CountryId,
                        Code = r.Code,
                        Name = r.Name,
                        SortOrder = r.SortOrder
                    })
                    .ToListAsync()));

            return app;
        }

        internal static CountryDto ToDto(Features.Countries.Entities.Country c) => new()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            IsBuiltIn = c.IsBuiltIn,
            SortOrder = c.SortOrder,
            Region1Name = c.Region1Name,
            Region1UseList = c.Region1UseList,
            DisplayRegion2 = c.DisplayRegion2,
            Region2Name = c.Region2Name
        };
    }
}
