namespace LeagueScheduler.Shared.Players;

/// <summary>Controls what contact information other players can see on this profile.</summary>
public enum PrivacyLevel
{
    /// <summary>Visible to all players and league administrators.</summary>
    Public = 0,
    /// <summary>Contact fields visible only to members of leagues the player belongs to.</summary>
    Restricted = 1,
    /// <summary>Not visible to other players; display name is always shown to league members.</summary>
    Private = 2
}

/// <summary>Optional gender identity, used for mixed-doubles scheduling when the league requires it.</summary>
public enum GenderIdentity
{
    Male = 0,
    Female = 1,
    NonBinary = 2,
    SelfDescribed = 3
}

public enum HandPreference
{
    Right = 0,
    Left = 1,
    /// <summary>Ambidextrous — effectively two forehands.</summary>
    Both = 2
}

public enum CourtSide
{
    Ad = 0,
    Deuce = 1
}

public enum CourtSurface
{
    Hard = 0,
    Clay = 1,
    Grass = 2
}
