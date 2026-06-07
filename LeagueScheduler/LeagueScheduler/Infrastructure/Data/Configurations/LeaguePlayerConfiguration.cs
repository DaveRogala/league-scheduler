using LeagueScheduler.Features.Leagues.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class LeaguePlayerConfiguration : AuditableEntityConfiguration<LeaguePlayer>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<LeaguePlayer> e)
        {
            e.HasKey(lp => new { lp.LeagueId, lp.SeasonPlayerId });
            e.HasOne(lp => lp.League)
                .WithMany(l => l.LeaguePlayers)
                .HasForeignKey(lp => lp.LeagueId);
            e.HasOne(lp => lp.SeasonPlayer)
                .WithMany(p => p.LeaguePlayers)
                .HasForeignKey(lp => lp.SeasonPlayerId);
        }
    }
}
