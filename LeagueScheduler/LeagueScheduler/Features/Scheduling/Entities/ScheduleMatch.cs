using LeagueScheduler.Features.Courts.Entities;

namespace LeagueScheduler.Features.Scheduling.Entities
{
    public class ScheduleMatch
    {
        public Guid Id { get; set; }
        public Guid ScheduleResultId { get; set; }
        public ScheduleResult ScheduleResult { get; set; } = null!;
        public DateTime Date { get; set; }
        // Court number within the season (1-based); retained until scheduling-v2-dtos
        // maps CourtId from the SeasonCourts ordered list
        public int CourtNumber { get; set; }
        // Nullable until scheduling DTOs are updated to pass Court entities
        public Guid? CourtId { get; set; }
        public Court? Court { get; set; }
        public List<Guid> PlayerIds { get; set; } = [];
    }
}
