using LeagueScheduler.Shared.Scheduling;
using LeagueScheduler.Features.Leagues.Entities;

namespace LeagueScheduler.Features.Players.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double PreferencePercent { get; set; }
        public PlayerRole Role { get; set; } = PlayerRole.Regular;
        public NudgePreference Nudge { get; set; } = NudgePreference.None;
        public List<DateTime> UnavailableDates { get; set; } = [];

        public List<LeaguePlayer> LeaguePlayers { get; set; } = [];
    }
}
