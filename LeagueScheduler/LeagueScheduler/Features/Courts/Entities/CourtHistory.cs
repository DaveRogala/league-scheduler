using LeagueScheduler.Shared.Courts;

namespace LeagueScheduler.Features.Courts.Entities;

public class CourtHistory
{
    public Guid Id { get; set; }
    public Guid CourtId { get; set; }
    public string Operation { get; set; } = string.Empty;
    public DateTimeOffset ChangedAt { get; set; }
    public Guid? ChangedById { get; set; }

    public string Name { get; set; } = string.Empty;
    public CourtType Type { get; set; }
    public int NumberOfCourts { get; set; }
    public bool Lighted { get; set; }
    public string? HoursOfOperation { get; set; }
    public string? AccessNotes { get; set; }
    public string? Description { get; set; }
    public string? Condition { get; set; }
    public Guid? AddressId { get; set; }
}
