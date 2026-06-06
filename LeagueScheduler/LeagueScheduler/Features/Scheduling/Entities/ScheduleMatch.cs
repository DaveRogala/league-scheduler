namespace LeagueScheduler.Features.Scheduling.Entities
{
    public class ScheduleMatch
    {
        public Guid Id { get; set; }
        public Guid ScheduleResultId { get; set; }
        public ScheduleResult ScheduleResult { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Court { get; set; }
        public List<Guid> PlayerIds { get; set; } = [];
    }
}
