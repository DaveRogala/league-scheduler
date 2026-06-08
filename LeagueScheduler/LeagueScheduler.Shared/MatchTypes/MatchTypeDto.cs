namespace LeagueScheduler.Shared.MatchTypes
{
    public class MatchTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MinPlayersPerCourt { get; set; }
        public int MaxPlayersPerCourt { get; set; }
        public bool MustHaveEvenPlayers { get; set; }
        public bool IsBuiltIn { get; set; }
        public int SortOrder { get; set; }
    }
}
