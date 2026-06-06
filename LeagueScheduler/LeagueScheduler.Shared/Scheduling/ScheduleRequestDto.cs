using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public record ScheduleRequestDto
    {
        public LeagueDto League { get; init; } = new LeagueDto();
        public List<PlayerDto> Players { get; init; } = new();
        public int MaxSwapIterations { get; init; } = 1000;
        public int? FairnessTolerance { get; init; }
    }
}
