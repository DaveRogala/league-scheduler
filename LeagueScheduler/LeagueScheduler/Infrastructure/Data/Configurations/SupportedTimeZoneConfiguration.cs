using LeagueScheduler.Features.Admin.TimeZones.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class SupportedTimeZoneConfiguration : IEntityTypeConfiguration<SupportedTimeZone>
    {
        public void Configure(EntityTypeBuilder<SupportedTimeZone> e)
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Id).HasMaxLength(100);
            e.Property(t => t.DisplayName).IsRequired().HasMaxLength(200);
        }
    }
}
