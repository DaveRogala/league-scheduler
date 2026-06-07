using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public record ScheduleResultDto
    {
        public Guid Id { get; init; }
        public Guid? SeasonId { get; init; }
        public List<MatchDto> Matches { get; init; } = new();
        public Dictionary<Guid, int> AssignedCounts { get; init; } = new();
        public Dictionary<Guid, int> TargetCounts { get; init; } = new();
        public int FairnessToleranceUsed { get; set; }
        public List<string> Conflicts { get; init; } = new();
    }
}
