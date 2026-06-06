using LeagueScheduler.Features.Common.Entities;
using LeagueScheduler.Features.Courts.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Common;
using LeagueScheduler.Shared.Courts;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Courts
{
    public static class CourtEndpoints
    {
        public static IEndpointRouteBuilder MapCourtEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/courts").RequireAuthorization();

            g.MapGet("", async (AppDbContext db) =>
                Results.Ok(await db.Courts
                    .Include(c => c.Address)
                    .OrderBy(c => c.Name)
                    .Select(c => ToDto(c))
                    .ToListAsync()));

            g.MapGet("{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var court = await db.Courts.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
                return court is not null ? Results.Ok(ToDto(court)) : Results.NotFound();
            });

            g.MapPost("", async (CourtDto dto, AppDbContext db) =>
            {
                var court = new Court
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name.Trim(),
                    Type = dto.Type,
                    NumberOfCourts = dto.NumberOfCourts,
                    Lighted = dto.Lighted,
                    HoursOfOperation = dto.HoursOfOperation?.Trim(),
                    AccessNotes = dto.AccessNotes?.Trim(),
                    Description = dto.Description?.Trim(),
                    Condition = dto.Condition?.Trim()
                };

                if (dto.Address is not null && HasData(dto.Address))
                {
                    court.Address = FromDto(dto.Address);
                    court.Address.Id = Guid.NewGuid();
                }

                db.Courts.Add(court);
                await db.SaveChangesAsync();
                return Results.Created($"/api/courts/{court.Id}", ToDto(court));
            });

            g.MapPut("{id:guid}", async (Guid id, CourtDto dto, AppDbContext db) =>
            {
                var court = await db.Courts.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
                if (court is null) return Results.NotFound();

                court.Name = dto.Name.Trim();
                court.Type = dto.Type;
                court.NumberOfCourts = dto.NumberOfCourts;
                court.Lighted = dto.Lighted;
                court.HoursOfOperation = dto.HoursOfOperation?.Trim();
                court.AccessNotes = dto.AccessNotes?.Trim();
                court.Description = dto.Description?.Trim();
                court.Condition = dto.Condition?.Trim();

                if (dto.Address is not null && HasData(dto.Address))
                {
                    if (court.Address is null)
                    {
                        court.Address = FromDto(dto.Address);
                        court.Address.Id = Guid.NewGuid();
                    }
                    else
                    {
                        ApplyToEntity(dto.Address, court.Address);
                    }
                }
                else if (court.Address is not null)
                {
                    db.Addresses.Remove(court.Address);
                    court.Address = null;
                    court.AddressId = null;
                }

                await db.SaveChangesAsync();
                return Results.Ok(ToDto(court));
            });

            g.MapDelete("{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var court = await db.Courts.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
                if (court is null) return Results.NotFound();
                if (court.Address is not null) db.Addresses.Remove(court.Address);
                db.Courts.Remove(court);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }

        private static bool HasData(AddressDto a) =>
            !string.IsNullOrWhiteSpace(a.Line1) ||
            !string.IsNullOrWhiteSpace(a.Line2) ||
            !string.IsNullOrWhiteSpace(a.Line3) ||
            !string.IsNullOrWhiteSpace(a.Locality) ||
            !string.IsNullOrWhiteSpace(a.AdminArea) ||
            !string.IsNullOrWhiteSpace(a.SubAdminArea) ||
            !string.IsNullOrWhiteSpace(a.PostalCode) ||
            !string.IsNullOrWhiteSpace(a.CountryCode);

        private static Address FromDto(AddressDto a) => new()
        {
            Line1 = a.Line1?.Trim(),
            Line2 = a.Line2?.Trim(),
            Line3 = a.Line3?.Trim(),
            Locality = a.Locality?.Trim(),
            AdminArea = a.AdminArea?.Trim(),
            SubAdminArea = a.SubAdminArea?.Trim(),
            PostalCode = a.PostalCode?.Trim(),
            CountryCode = a.CountryCode?.Trim()?.ToUpperInvariant(),
            VisibleFields = a.VisibleFields
        };

        private static void ApplyToEntity(AddressDto a, Address entity)
        {
            entity.Line1 = a.Line1?.Trim();
            entity.Line2 = a.Line2?.Trim();
            entity.Line3 = a.Line3?.Trim();
            entity.Locality = a.Locality?.Trim();
            entity.AdminArea = a.AdminArea?.Trim();
            entity.SubAdminArea = a.SubAdminArea?.Trim();
            entity.PostalCode = a.PostalCode?.Trim();
            entity.CountryCode = a.CountryCode?.Trim()?.ToUpperInvariant();
            entity.VisibleFields = a.VisibleFields;
        }

        private static CourtDto ToDto(Court c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Type = c.Type,
            NumberOfCourts = c.NumberOfCourts,
            Lighted = c.Lighted,
            HoursOfOperation = c.HoursOfOperation,
            AccessNotes = c.AccessNotes,
            Description = c.Description,
            Condition = c.Condition,
            Address = c.Address is not null ? new AddressDto
            {
                Id = c.Address.Id,
                Line1 = c.Address.Line1,
                Line2 = c.Address.Line2,
                Line3 = c.Address.Line3,
                Locality = c.Address.Locality,
                AdminArea = c.Address.AdminArea,
                SubAdminArea = c.Address.SubAdminArea,
                PostalCode = c.Address.PostalCode,
                CountryCode = c.Address.CountryCode,
                VisibleFields = c.Address.VisibleFields
            } : null
        };
    }
}
