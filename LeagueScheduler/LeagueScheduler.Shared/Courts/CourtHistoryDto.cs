namespace LeagueScheduler.Shared.Courts;

public record CourtHistoryDto
{
    public Guid Id { get; init; }
    public Guid CourtId { get; init; }
    public string Operation { get; init; } = string.Empty;
    public DateTimeOffset ChangedAt { get; init; }
    public Guid? ChangedById { get; init; }
    public string? ChangedByName { get; init; }

    public string Name { get; init; } = string.Empty;
    public CourtType Type { get; init; }
    public int NumberOfCourts { get; init; }
    public bool Lighted { get; init; }
    public string? HoursOfOperation { get; init; }
    public string? AccessNotes { get; init; }
    public string? Description { get; init; }
    public string? Condition { get; init; }
    public Guid? AddressId { get; init; }
}

public record CourtHistoryPageDto
{
    public List<CourtHistoryDto> Items { get; init; } = [];
    public int TotalCount { get; init; }
}
