using LeagueScheduler.Shared.Common;

namespace LeagueScheduler.Features.Common.Entities
{
    public class Address
    {
        public Guid Id { get; set; }

        // ISO 19160-4 thoroughfare / delivery lines
        public string? Line1 { get; set; }       // Street number + name, PO Box
        public string? Line2 { get; set; }       // Unit, suite, apt, floor, building
        public string? Line3 { get; set; }       // Additional delivery info

        // ISO 19160-4 place components
        public string? Locality { get; set; }    // City / municipality
        public string? AdminArea { get; set; }   // State / province / territory / prefecture
        public string? SubAdminArea { get; set; } // County / district / arrondissement

        // Postal
        public string? PostalCode { get; set; }

        // ISO 3166-1 alpha-2 country code
        public string? CountryCode { get; set; }

        // Controls which fields are shown in the UI for this address instance
        public AddressField VisibleFields { get; set; } = AddressField.Standard;
    }
}
