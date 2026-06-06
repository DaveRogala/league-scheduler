using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public record MatchDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime Date { get; init; }
        public Guid CourtId { get; init; }
        public string CourtName { get; init; } = string.Empty;
        public List<Guid> PlayerIds { get; init; } = new();
    }
}
