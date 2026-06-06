using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public record ScheduleRequestDto
    {
        public SeasonDto Season { get; init; } = new();
        public List<CourtDto> Courts { get; init; } = new();
        public List<PlayerDto> Players { get; init; } = new();
        public int? FairnessTolerance { get; init; }
    }
}
