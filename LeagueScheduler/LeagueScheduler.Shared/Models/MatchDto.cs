using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Models
{
    public record MatchDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime Date { get; init; }
        public int Court { get; init; }
        // Player IDs assigned to this match (2 for singles, 4 for doubles)
        public List<Guid> PlayerIds { get; init; } = new();
    }
}
