namespace LeagueScheduler.Features.Countries.Entities
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsBuiltIn { get; set; }
        public int SortOrder { get; set; }
        public string Region1Name { get; set; } = "State/Province";
        public bool Region1UseList { get; set; }
        public bool DisplayRegion2 { get; set; }
        public string? Region2Name { get; set; }
        public ICollection<CountryRegion> Regions { get; set; } = [];
    }
}
