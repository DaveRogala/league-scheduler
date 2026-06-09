using LeagueScheduler.Features.Common.Entities;
using LeagueScheduler.Features.Players.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Common;
using LeagueScheduler.Shared.Players;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LeagueScheduler.Features.Players;

public static class PlayerProfileEndpoints
{
    public static IEndpointRouteBuilder MapPlayerProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/player-profile").RequireAuthorization();

        g.MapGet("", async (ClaimsPrincipal principal, AppDbContext db) =>
        {
            var userId = GetUserId(principal);
            if (userId == Guid.Empty) return Results.Unauthorized();

            var profile = await db.PlayerProfiles
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return Results.Ok(profile is null ? new PlayerProfileDto() : ToDto(profile));
        });

        g.MapPut("", async (PlayerProfileDto dto, ClaimsPrincipal principal, AppDbContext db) =>
        {
            var userId = GetUserId(principal);
            if (userId == Guid.Empty) return Results.Unauthorized();

            var profile = await db.PlayerProfiles
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile is null)
            {
                profile = new PlayerProfile { UserId = userId };
                db.PlayerProfiles.Add(profile);
            }

            // Privacy
            profile.PrivacyLevel = dto.PrivacyLevel;
            profile.ShowEmail = dto.ShowEmail;

            // Phone
            profile.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim();
            profile.ShowPhone = dto.ShowPhone;

            // Address visibility
            profile.ShowAddress = dto.ShowAddress;
            profile.ShowCityPostalOnly = dto.ShowCityPostalOnly;

            // Upsert or clear address
            if (dto.Address is not null && HasAddressContent(dto.Address))
            {
                if (profile.Address is null)
                {
                    profile.Address = new Address();
                    db.Addresses.Add(profile.Address);
                }
                ApplyAddress(profile.Address, dto.Address);
            }
            else if (profile.Address is not null)
            {
                db.Addresses.Remove(profile.Address);
                profile.Address = null;
                profile.AddressId = null;
            }

            // Gender
            profile.Gender = dto.Gender;
            profile.GenderDescription = dto.Gender == GenderIdentity.SelfDescribed
                ? dto.GenderDescription?.Trim()
                : null;

            // Ratings
            profile.NtrpRating = dto.NtrpRating;
            profile.WtnSingles = dto.WtnSingles;
            profile.WtnDoubles = dto.WtnDoubles;
            profile.UtrRating = dto.UtrRating;

            // Preferences
            profile.HandPreference = dto.HandPreference;
            profile.PreferredSide = dto.PreferredSide;
            profile.PreferredSurface = dto.PreferredSurface;

            await db.SaveChangesAsync();
            return Results.Ok(ToDto(profile));
        });

        return app;
    }

    private static Guid GetUserId(ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : Guid.Empty;
    }

    private static bool HasAddressContent(AddressDto a) =>
        !string.IsNullOrWhiteSpace(a.Line1) ||
        !string.IsNullOrWhiteSpace(a.Locality) ||
        !string.IsNullOrWhiteSpace(a.PostalCode) ||
        a.CountryId.HasValue;

    private static void ApplyAddress(Address target, AddressDto src)
    {
        target.Line1 = src.Line1?.Trim();
        target.Line2 = src.Line2?.Trim();
        target.Line3 = src.Line3?.Trim();
        target.Locality = src.Locality?.Trim();
        target.AdminArea = src.AdminArea?.Trim();
        target.AdminAreaId = src.AdminAreaId;
        target.SubAdminArea = src.SubAdminArea?.Trim();
        target.PostalCode = src.PostalCode?.Trim();
        target.CountryId = src.CountryId;
    }

    private static PlayerProfileDto ToDto(PlayerProfile p) => new()
    {
        PrivacyLevel = p.PrivacyLevel,
        ShowEmail = p.ShowEmail,
        PhoneNumber = p.PhoneNumber,
        ShowPhone = p.ShowPhone,
        Address = p.Address is null ? null : new AddressDto
        {
            Id = p.Address.Id,
            Line1 = p.Address.Line1,
            Line2 = p.Address.Line2,
            Line3 = p.Address.Line3,
            Locality = p.Address.Locality,
            AdminArea = p.Address.AdminArea,
            AdminAreaId = p.Address.AdminAreaId,
            SubAdminArea = p.Address.SubAdminArea,
            PostalCode = p.Address.PostalCode,
            CountryId = p.Address.CountryId
        },
        ShowAddress = p.ShowAddress,
        ShowCityPostalOnly = p.ShowCityPostalOnly,
        Gender = p.Gender,
        GenderDescription = p.GenderDescription,
        NtrpRating = p.NtrpRating,
        WtnSingles = p.WtnSingles,
        WtnDoubles = p.WtnDoubles,
        UtrRating = p.UtrRating,
        HandPreference = p.HandPreference,
        PreferredSide = p.PreferredSide,
        PreferredSurface = p.PreferredSurface
    };
}
