using LeagueScheduler.Features.Courts.Entities;

namespace LeagueScheduler.Features.Seasons.Entities
{
    public class SeasonCourt
    {
        public Guid SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        public Guid CourtId { get; set; }
        public Court Court { get; set; } = null!;
        // Determines which court number (1, 2, …) this maps to in the schedule
        public int SortOrder { get; set; }
    }
}
