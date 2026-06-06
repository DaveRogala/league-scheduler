namespace LeagueScheduler.Shared.Common
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }
        public string? Locality { get; set; }
        public string? AdminArea { get; set; }
        public string? SubAdminArea { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryCode { get; set; }
        public AddressField VisibleFields { get; set; } = AddressField.Standard;
    }
}
