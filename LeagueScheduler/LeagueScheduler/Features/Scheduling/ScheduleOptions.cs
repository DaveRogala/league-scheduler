namespace LeagueScheduler.Features.Scheduling
{
    public class ScheduleOptions
    {
        public int DefaultFairnessTolerance { get; set; } = 1;
        public string? DataFolder { get; set; } = "Data";
    }
}
