using LeagueScheduler.Features.Countries.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class CountryRegionConfiguration : IEntityTypeConfiguration<CountryRegion>
    {
        public void Configure(EntityTypeBuilder<CountryRegion> e)
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(r => r.Code).IsRequired().HasMaxLength(10);
            e.Property(r => r.Name).IsRequired().HasMaxLength(100);
            e.HasIndex(r => new { r.CountryId, r.Code }).IsUnique();
        }
    }
}
