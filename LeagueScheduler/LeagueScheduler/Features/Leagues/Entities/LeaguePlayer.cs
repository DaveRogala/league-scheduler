using LeagueScheduler.Features.Players.Entities;

namespace LeagueScheduler.Features.Leagues.Entities
{
    public class LeaguePlayer
    {
        public Guid LeagueId { get; set; }
        public League League { get; set; } = null!;
        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public bool IsAdmin { get; set; }
        // False only when the league requires approval; defaults to true for open leagues
        public bool IsApproved { get; set; } = true;
    }
}
