using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Scheduling
{
    public enum PlayerRole
    {
        Regular,
        AsNeeded,
        OnCall
    }

    public enum NudgePreference
    {
        None,
        Above,
        Below
    }

    public record PlayerDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
        public double PreferencePercent { get; init; }
        public PlayerRole Role { get; init; } = PlayerRole.Regular;
        public NudgePreference Nudge { get; init; } = NudgePreference.None;
        public List<DateTime> UnavailableDates { get; init; } = new();
        public List<Guid> LeagueIds { get; init; } = new();
    }
}
