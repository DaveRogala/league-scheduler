using LeagueScheduler.Features.Leagues.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class LeaguePlayerConfiguration : IEntityTypeConfiguration<LeaguePlayer>
    {
        public void Configure(EntityTypeBuilder<LeaguePlayer> e)
        {
            e.HasKey(lp => new { lp.LeagueId, lp.PlayerId });
            e.HasOne(lp => lp.League)
                .WithMany(l => l.LeaguePlayers)
                .HasForeignKey(lp => lp.LeagueId);
            e.HasOne(lp => lp.Player)
                .WithMany(p => p.LeaguePlayers)
                .HasForeignKey(lp => lp.PlayerId);
        }
    }
}
