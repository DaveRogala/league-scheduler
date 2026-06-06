using LeagueScheduler.Shared.Common;

namespace LeagueScheduler.Shared.Courts
{
    public class CourtDto
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
        public AddressDto? Address { get; set; }
    }
}
