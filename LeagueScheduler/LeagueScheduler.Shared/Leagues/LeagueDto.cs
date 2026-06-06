using LeagueScheduler.Shared.Scheduling;
using MatchType = LeagueScheduler.Shared.Scheduling.MatchType;

namespace LeagueScheduler.Shared.Leagues
{
    public class LeagueDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public LeagueMode Mode { get; set; } = LeagueMode.Recreational;
        public MatchType MatchType { get; set; } = MatchType.Doubles;
        public bool RequireApprovalToJoin { get; set; }
    }
}
