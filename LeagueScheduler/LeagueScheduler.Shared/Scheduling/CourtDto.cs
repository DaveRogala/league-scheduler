using System;

namespace LeagueScheduler.Shared.Scheduling
{
    public record CourtDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
    }
}
