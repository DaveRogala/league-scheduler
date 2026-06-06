using LeagueScheduler.Shared.Courts;
using LeagueScheduler.Features.Seasons.Entities;

namespace LeagueScheduler.Features.Courts.Entities
{
    public class Court
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateProvince { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public CourtType Type { get; set; } = CourtType.Outdoor;
        public int NumberOfCourts { get; set; } = 1;
        public string? AccessNotes { get; set; }
        public string? HoursOfOperation { get; set; }
        public bool Lighted { get; set; }
        public string? Description { get; set; }
        public string? Condition { get; set; }

        public List<SeasonCourt> SeasonCourts { get; set; } = [];
    }
}
