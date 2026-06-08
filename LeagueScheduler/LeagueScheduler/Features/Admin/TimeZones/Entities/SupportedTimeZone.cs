namespace LeagueScheduler.Features.Admin.TimeZones.Entities
{
    public class SupportedTimeZone
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public double UtcOffsetHours { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
