using LeagueScheduler.Shared.MatchTypes;

namespace LeagueScheduler.Shared.Leagues
{
    public class LeagueDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public LeagueMode Mode { get; set; } = LeagueMode.Recreational;
        public Guid MatchTypeId { get; set; }
        public MatchTypeDto? MatchType { get; set; }
        public bool RequireApprovalToJoin { get; set; }
    }
}
