using LeagueScheduler.Features.Seasons.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class SeasonCourtConfiguration : IEntityTypeConfiguration<SeasonCourt>
    {
        public void Configure(EntityTypeBuilder<SeasonCourt> e)
        {
            e.HasKey(sc => new { sc.SeasonId, sc.CourtId });
            e.HasOne(sc => sc.Season)
                .WithMany(s => s.SeasonCourts)
                .HasForeignKey(sc => sc.SeasonId);
            e.HasOne(sc => sc.Court)
                .WithMany(c => c.SeasonCourts)
                .HasForeignKey(sc => sc.CourtId);
        }
    }
}
