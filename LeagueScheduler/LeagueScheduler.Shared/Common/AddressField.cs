namespace LeagueScheduler.Shared.Common
{
    [Flags]
    public enum AddressField
    {
        None = 0,
        Line1 = 1 << 0,
        Line2 = 1 << 1,
        Line3 = 1 << 2,
        Locality = 1 << 3,      // city / municipality
        AdminArea = 1 << 4,     // state / province / territory
        SubAdminArea = 1 << 5,  // county / district
        PostalCode = 1 << 6,
        Country = 1 << 7,

        Standard = Line1 | Line2 | Locality | AdminArea | PostalCode | Country,
        Full = Line1 | Line2 | Line3 | Locality | AdminArea | SubAdminArea | PostalCode | Country
    }
}
