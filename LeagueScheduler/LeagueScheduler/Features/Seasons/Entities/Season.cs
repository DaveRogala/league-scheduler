using LeagueScheduler.Features.Leagues.Entities;
using LeagueScheduler.Features.Scheduling.Entities;
using LeagueScheduler.Infrastructure.Audit;

namespace LeagueScheduler.Features.Seasons.Entities
{
    public class Season : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid LeagueId { get; set; }
        public League League { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; } = [];
        public List<DateTime> NonPlayDates { get; set; } = [];

        public List<SeasonCourt> SeasonCourts { get; set; } = [];
        public List<PrePlannedEvent> PrePlannedEvents { get; set; } = [];
        public List<ScheduleResult> ScheduleResults { get; set; } = [];
    }
}
