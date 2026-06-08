using LeagueScheduler.Features.Countries;
using LeagueScheduler.Features.Countries.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Admin.Countries
{
    public static class AdminCountryEndpoints
    {
        public static IEndpointRouteBuilder MapAdminCountryEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/admin/countries").RequireAuthorization();

            g.MapPost("", async (CountryDto dto, AppDbContext db) =>
            {
                if (await db.Countries.AnyAsync(c => c.Code == dto.Code.ToUpperInvariant()))
                    return Results.Conflict("A country with that code already exists.");

                var country = new Country
                {
                    Code = dto.Code.Trim().ToUpperInvariant(),
                    Name = dto.Name.Trim(),
                    SortOrder = dto.SortOrder,
                    Region1Name = dto.Region1Name.Trim(),
                    Region1UseList = dto.Region1UseList,
                    DisplayRegion2 = dto.DisplayRegion2,
                    Region2Name = dto.Region2Name?.Trim()
                };
                db.Countries.Add(country);
                await db.SaveChangesAsync();
                return Results.Created($"/api/countries", CountryEndpoints.ToDto(country));
            });

            g.MapPut("{id:guid}", async (Guid id, CountryDto dto, AppDbContext db) =>
            {
                var country = await db.Countries.FindAsync(id);
                if (country is null) return Results.NotFound();

                // Allow editing config for all countries; code is immutable for built-ins.
                if (!country.IsBuiltIn)
                    country.Code = dto.Code.Trim().ToUpperInvariant();

                country.Name = dto.Name.Trim();
                country.SortOrder = dto.SortOrder;
                country.Region1Name = dto.Region1Name.Trim();
                country.Region1UseList = dto.Region1UseList;
                country.DisplayRegion2 = dto.DisplayRegion2;
                country.Region2Name = dto.Region2Name?.Trim();

                await db.SaveChangesAsync();
                return Results.Ok(CountryEndpoints.ToDto(country));
            });

            g.MapDelete("{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var country = await db.Countries.FindAsync(id);
                if (country is null) return Results.NotFound();
                if (country.IsBuiltIn) return Results.Conflict("Built-in countries cannot be removed.");

                db.Countries.Remove(country);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // --- regions ---

            g.MapPost("{id:guid}/regions", async (Guid id, CountryRegionDto dto, AppDbContext db) =>
            {
                if (!await db.Countries.AnyAsync(c => c.Id == id))
                    return Results.NotFound();

                var region = new CountryRegion
                {
                    CountryId = id,
                    Code = dto.Code.Trim().ToUpperInvariant(),
                    Name = dto.Name.Trim(),
                    SortOrder = dto.SortOrder
                };
                db.CountryRegions.Add(region);
                await db.SaveChangesAsync();
                return Results.Created($"/api/countries/{id}/regions", new CountryRegionDto
                {
                    Id = region.Id,
                    CountryId = region.CountryId,
                    Code = region.Code,
                    Name = region.Name,
                    SortOrder = region.SortOrder
                });
            });

            g.MapDelete("{id:guid}/regions/{regionId:guid}", async (Guid id, Guid regionId, AppDbContext db) =>
            {
                var region = await db.CountryRegions.FirstOrDefaultAsync(r => r.Id == regionId && r.CountryId == id);
                if (region is null) return Results.NotFound();

                db.CountryRegions.Remove(region);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }
    }
}
