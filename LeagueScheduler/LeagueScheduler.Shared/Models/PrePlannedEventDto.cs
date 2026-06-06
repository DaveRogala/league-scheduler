using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Models
{
    public record PrePlannedEventDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime Date { get; init; }
        public int Court { get; init; }
        // PlayerIds involved in this pre-planned event (may be empty for blocked court)
        public List<Guid> PlayerIds { get; init; } = new();
        public bool IsMatch { get; init; } = true;
    }
}
