using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Models
{
    public record ScheduleRequestDto
    {
        public LeagueDto League { get; init; } = new LeagueDto();
        public List<PlayerDto> Players { get; init; } = new();
        // Optional: enforce exact targets or allow tolerance
        public int MaxSwapIterations { get; init; } = 1000;
        // Optional override for fairness tolerance (in slots). If null, server default applies.
        public int? FairnessTolerance { get; init; }
    }
}
