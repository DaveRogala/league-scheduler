using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MatchTypeEntity = LeagueScheduler.Features.MatchTypes.Entities.MatchType;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class MatchTypeConfiguration : IEntityTypeConfiguration<MatchTypeEntity>
    {
        public static readonly Guid SinglesId       = new("4b6f3f9e-0002-4000-a000-000000000001");
        public static readonly Guid DoublesId       = new("4b6f3f9e-0002-4000-a000-000000000002");
        public static readonly Guid CanadianDoublesId = new("4b6f3f9e-0002-4000-a000-000000000003");
        public static readonly Guid DinglesId       = new("4b6f3f9e-0002-4000-a000-000000000004");
        public static readonly Guid RoundRobinId    = new("4b6f3f9e-0002-4000-a000-000000000005");

        public void Configure(EntityTypeBuilder<MatchTypeEntity> e)
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(m => m.Name).IsRequired().HasMaxLength(100);
            e.Property(m => m.Description).HasMaxLength(1000);
            e.HasIndex(m => m.Name).IsUnique();

            e.HasData(
                new MatchTypeEntity
                {
                    Id = SinglesId, Name = "Singles", Description = "Singles",
                    MinPlayersPerCourt = 2, MaxPlayersPerCourt = 2, MustHaveEvenPlayers = true,
                    IsBuiltIn = true, SortOrder = 1
                },
                new MatchTypeEntity
                {
                    Id = DoublesId, Name = "Doubles", Description = "Doubles",
                    MinPlayersPerCourt = 4, MaxPlayersPerCourt = 4, MustHaveEvenPlayers = true,
                    IsBuiltIn = true, SortOrder = 2
                },
                new MatchTypeEntity
                {
                    Id = CanadianDoublesId, Name = "Canadian Doubles",
                    Description = "Three player tennis where the single player takes on two players. " +
                        "The single player can hit into the entire doubles court where the doubles side can only " +
                        "hit into the singles court. Additional rule modifications include rotating players so that " +
                        "the single player is always serving and counting only wins when serving with a 'first to N wins' rule.",
                    MinPlayersPerCourt = 3, MaxPlayersPerCourt = 3, MustHaveEvenPlayers = false,
                    IsBuiltIn = true, SortOrder = 3
                },
                new MatchTypeEntity
                {
                    Id = DinglesId, Name = "Dingles",
                    Description = "Also known as 'two-ball live.' A point starts with both players on one side " +
                        "simultaneously feeding a ball and playing a singles point until one pair misses, at which " +
                        "point a player on the pair that misses yells 'Dingles!' or 'Live!' where it becomes a " +
                        "standard doubles point. Singles play can be down the line or cross-court. Simultaneous " +
                        "'Dingles' (both balls miss at the same time) results in no-point and the point starts again. " +
                        "Sides alternate feeding either every point or after an agreed upon number of points.",
                    MinPlayersPerCourt = 4, MaxPlayersPerCourt = 4, MustHaveEvenPlayers = true,
                    IsBuiltIn = true, SortOrder = 4
                },
                new MatchTypeEntity
                {
                    Id = RoundRobinId, Name = "Round-Robin",
                    Description = "A format where there are more players than court slots. Players rotate on an " +
                        "agreed upon basis, usually after a fixed time or fixed number of games. If score is kept " +
                        "it is usually on a player-by-player basis by counting games that the player's team won. " +
                        "If there are only four players then the players can either play standard doubles matches or rotate.",
                    MinPlayersPerCourt = 4, MaxPlayersPerCourt = 8, MustHaveEvenPlayers = false,
                    IsBuiltIn = true, SortOrder = 5
                });
        }
    }
}
