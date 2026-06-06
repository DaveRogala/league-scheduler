using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Models
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
        // Days of week the league plays (e.g., Saturday)
        public List<DayOfWeek> DaysOfWeek { get; init; } = new();
        // Non-play dates such as holidays or club-closed dates
        public List<DateTime> NonPlayDates { get; init; } = new();
        // Pre-planned events that must be honored
        public List<PrePlannedEventDto> PrePlannedEvents { get; init; } = new();
    }
}
