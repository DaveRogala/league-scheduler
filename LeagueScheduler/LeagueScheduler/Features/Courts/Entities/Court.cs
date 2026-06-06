using LeagueScheduler.Shared.Courts;
using LeagueScheduler.Features.Common.Entities;
using LeagueScheduler.Features.Seasons.Entities;

namespace LeagueScheduler.Features.Courts.Entities
{
    public class Court
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CourtType Type { get; set; } = CourtType.Outdoor;
        public int NumberOfCourts { get; set; } = 1;
        public bool Lighted { get; set; }
        public string? HoursOfOperation { get; set; }
        public string? AccessNotes { get; set; }
        public string? Description { get; set; }
        public string? Condition { get; set; }

        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }

        public List<SeasonCourt> SeasonCourts { get; set; } = [];
    }
}
