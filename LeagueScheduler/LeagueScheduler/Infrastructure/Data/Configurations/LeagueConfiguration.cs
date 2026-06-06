using LeagueScheduler.Features.Leagues.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> e)
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.Name).IsRequired().HasMaxLength(200);
            e.Property(l => l.Mode).HasConversion<string>();
            e.Property(l => l.MatchType).HasConversion<string>();
        }
    }
}
