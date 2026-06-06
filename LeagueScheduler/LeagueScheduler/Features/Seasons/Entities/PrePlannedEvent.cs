using LeagueScheduler.Features.Courts.Entities;

namespace LeagueScheduler.Features.Seasons.Entities
{
    public class PrePlannedEvent
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public DateTime Date { get; set; }
        public Guid? CourtId { get; set; }
        public Court? Court { get; set; }
        public List<Guid> PlayerIds { get; set; } = [];
        public bool IsMatch { get; set; } = true;
    }
}
