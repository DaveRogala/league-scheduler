using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public enum MatchType
    {
        Singles,
        Doubles
    }

    public record LeagueDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
        public MatchType MatchType { get; init; } = MatchType.Doubles;
        public int Courts { get; init; } = 1;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public List<DayOfWeek> DaysOfWeek { get; init; } = new();
        public List<DateTime> NonPlayDates { get; init; } = new();
        public List<PrePlannedEventDto> PrePlannedEvents { get; init; } = new();
    }
}
