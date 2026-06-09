using LeagueScheduler.Shared.Common;

namespace LeagueScheduler.Shared.Players;

public record PlayerProfileDto
{
    // Privacy
    public PrivacyLevel PrivacyLevel { get; init; } = PrivacyLevel.Restricted;
    public bool ShowEmail { get; init; }

    // Phone
    public string? PhoneNumber { get; init; }
    public bool ShowPhone { get; init; }

    // Address
    public AddressDto? Address { get; init; }
    public bool ShowAddress { get; init; }
    public bool ShowCityPostalOnly { get; init; }

    // Gender (optional, used for mixed-doubles scheduling)
    public GenderIdentity? Gender { get; init; }
    public string? GenderDescription { get; init; }

    // Tennis ratings (all optional)
    public decimal? NtrpRating { get; init; }
    public decimal? WtnSingles { get; init; }
    public decimal? WtnDoubles { get; init; }
    public decimal? UtrRating { get; init; }

    // Playing preferences (all optional)
    public HandPreference? HandPreference { get; init; }
    public CourtSide? PreferredSide { get; init; }
    public CourtSurface? PreferredSurface { get; init; }
}
