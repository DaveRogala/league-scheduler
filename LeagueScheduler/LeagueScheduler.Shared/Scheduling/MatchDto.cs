using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public record MatchDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime Date { get; init; }
        public int Court { get; init; }
        public List<Guid> PlayerIds { get; init; } = new();
    }
}
