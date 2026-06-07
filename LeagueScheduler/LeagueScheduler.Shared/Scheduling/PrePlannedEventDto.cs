using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public record PrePlannedEventDto
    {
        public required Guid Id { get; init; }
        public DateTime Date { get; init; }
        public int Court { get; init; }
        public List<Guid> PlayerIds { get; init; } = new();
        public bool IsMatch { get; init; } = true;
    }
}
