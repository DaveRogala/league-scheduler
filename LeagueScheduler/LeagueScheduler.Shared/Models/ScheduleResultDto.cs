using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Models
{
    public record ScheduleResultDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public List<MatchDto> Matches { get; init; } = new();
        // Per-player assigned counts for easy summary
        public Dictionary<Guid, int> AssignedCounts { get; init; } = new();
        // Per-player target counts computed during scheduling (for comparison)
        public Dictionary<Guid, int> TargetCounts { get; init; } = new();
        // Fairness tolerance (slots) used during validation
        public int FairnessToleranceUsed { get; set; }
        public List<string> Conflicts { get; init; } = new();
    }
}
