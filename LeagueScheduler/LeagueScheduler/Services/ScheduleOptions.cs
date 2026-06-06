namespace LeagueScheduler.Services
{
    public class ScheduleOptions
    {
        // Default fairness tolerance in number of slots (e.g., +/- 1)
        public int DefaultFairnessTolerance { get; set; } = 1;
        // Folder to store JSON schedules
        public string? DataFolder { get; set; } = "Data";
    }
}
