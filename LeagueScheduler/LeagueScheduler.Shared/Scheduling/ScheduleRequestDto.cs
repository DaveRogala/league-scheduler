using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public record ScheduleRequestDto
    {
        public required SeasonDto Season { get; init; }
        public List<ScheduleCourtDto> Courts { get; init; } = new();
        public List<PlayerDto> Players { get; init; } = new();
        public int? FairnessTolerance { get; init; }
    }
}
