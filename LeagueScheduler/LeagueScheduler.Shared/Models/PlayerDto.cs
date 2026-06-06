using System;
using System.Collections.Generic;

namespace LeagueScheduler.Shared.Models
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
        // Percentage as a value between 0.0 and 1.0
        public double PreferencePercent { get; init; }
        public PlayerRole Role { get; init; } = PlayerRole.Regular;
        public NudgePreference Nudge { get; init; } = NudgePreference.None;
        // Dates the player is unavailable (date component used)
        public List<DateTime> UnavailableDates { get; init; } = new();
        // Optional: leagues the player participates in
        public List<Guid> LeagueIds { get; init; } = new();
    }
}
