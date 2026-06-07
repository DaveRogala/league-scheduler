namespace LeagueScheduler.Features.Countries.Entities
{
    public class CountryRegion
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; } = null!;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
