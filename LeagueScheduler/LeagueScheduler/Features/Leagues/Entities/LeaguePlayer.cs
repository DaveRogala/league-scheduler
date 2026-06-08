using LeagueScheduler.Features.SeasonPlayers.Entities;
using LeagueScheduler.Infrastructure.Audit;

namespace LeagueScheduler.Features.Leagues.Entities
{
    public class LeaguePlayer : AuditableEntity
    {
        public Guid LeagueId { get; set; }
        public League League { get; set; } = null!;
        public Guid SeasonPlayerId { get; set; }
        public SeasonPlayer SeasonPlayer { get; set; } = null!;
        public bool IsAdmin { get; set; }
        // False only when the league requires approval; defaults to true for open leagues
        public bool IsApproved { get; set; } = true;
    }
}
