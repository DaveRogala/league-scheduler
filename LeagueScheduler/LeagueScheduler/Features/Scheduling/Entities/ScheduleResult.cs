namespace LeagueScheduler.Features.Scheduling.Entities
{
    public class ScheduleResult
    {
        public Guid Id { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public int FairnessToleranceUsed { get; set; }
        public Dictionary<Guid, int> AssignedCounts { get; set; } = [];
        public Dictionary<Guid, int> TargetCounts { get; set; } = [];
        public List<string> Conflicts { get; set; } = [];
        public List<ScheduleMatch> Matches { get; set; } = [];
    }
}
