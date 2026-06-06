using LeagueScheduler.Shared.Leagues;
using LeagueScheduler.Shared.Scheduling;
using LeagueScheduler.Features.Seasons.Entities;

namespace LeagueScheduler.Features.Leagues.Entities
{
    public class League
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public LeagueMode Mode { get; set; } = LeagueMode.Recreational;
        public MatchType MatchType { get; set; } = MatchType.Doubles;
        public bool RequireApprovalToJoin { get; set; }

        public List<Season> Seasons { get; set; } = [];
        public List<LeaguePlayer> LeaguePlayers { get; set; } = [];
    }
}
