using LeagueScheduler.Features.Seasons.Entities;
using LeagueScheduler.Infrastructure.Audit;
using LeagueScheduler.Shared.Leagues;
using ServerMatchType = LeagueScheduler.Features.MatchTypes.Entities.MatchType;

namespace LeagueScheduler.Features.Leagues.Entities
{
    public class League : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public LeagueMode Mode { get; set; } = LeagueMode.Recreational;
        public Guid MatchTypeId { get; set; }
        public ServerMatchType MatchType { get; set; } = null!;
        public bool RequireApprovalToJoin { get; set; }

        public List<Season> Seasons { get; set; } = [];
        public List<LeaguePlayer> LeaguePlayers { get; set; } = [];
    }
}
