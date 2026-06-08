using LeagueScheduler.Features.Countries.Entities;
using LeagueScheduler.Shared.Common;

namespace LeagueScheduler.Features.Common.Entities
{
    public class Address
    {
        public Guid Id { get; set; }

        // ISO 19160-4 thoroughfare / delivery lines
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        // ISO 19160-4 place components
        public string? Locality { get; set; }
        public string? AdminArea { get; set; }       // free text for custom countries; synced from region name for list countries
        public Guid? AdminAreaId { get; set; }       // FK to CountryRegion when Region1UseList = true
        public CountryRegion? AdminAreaRegion { get; set; }
        public string? SubAdminArea { get; set; }

        // Postal
        public string? PostalCode { get; set; }

        // FK to Country table
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }

        // Controls which thoroughfare/postal fields are visible in the UI for this address instance
        public AddressField VisibleFields { get; set; } = AddressField.Standard;
    }
}
