using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Features.Common.Entities;
using LeagueScheduler.Shared.Players;

namespace LeagueScheduler.Features.Players.Entities;

public class PlayerProfile
{
    // UserId is both the PK and the FK to AppUser — true one-to-one.
    public Guid UserId { get; set; }

    // Privacy
    public PrivacyLevel PrivacyLevel { get; set; } = PrivacyLevel.Restricted;
    public bool ShowEmail { get; set; }

    // Phone
    public string? PhoneNumber { get; set; }
    public bool ShowPhone { get; set; }

    // Address (optional, separate entity shared with courts/leagues)
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
    public bool ShowAddress { get; set; }
    public bool ShowCityPostalOnly { get; set; }

    // Gender (optional, used for mixed-doubles scheduling)
    public GenderIdentity? Gender { get; set; }
    public string? GenderDescription { get; set; }

    // Tennis ratings (all optional)
    public decimal? NtrpRating { get; set; }
    public decimal? WtnSingles { get; set; }
    public decimal? WtnDoubles { get; set; }
    public decimal? UtrRating { get; set; }

    // Playing preferences (all optional)
    public HandPreference? HandPreference { get; set; }
    public CourtSide? PreferredSide { get; set; }
    public CourtSurface? PreferredSurface { get; set; }

    public AppUser User { get; set; } = null!;
}
