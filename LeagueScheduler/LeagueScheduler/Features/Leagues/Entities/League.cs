using LeagueScheduler.Shared.Leagues;
using LeagueScheduler.Features.Seasons.Entities;
using MatchType = LeagueScheduler.Shared.Scheduling.MatchType;

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
