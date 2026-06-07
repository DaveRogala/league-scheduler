using LeagueScheduler.Features.Countries.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public static readonly Guid UsId = new("4b6f3f9e-0001-4000-a000-000000000001");
        public static readonly Guid CaId = new("4b6f3f9e-0001-4000-a000-000000000002");

        public void Configure(EntityTypeBuilder<Country> e)
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(c => c.Code).IsRequired().HasMaxLength(3);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.Region1Name).IsRequired().HasMaxLength(50);
            e.Property(c => c.Region2Name).HasMaxLength(50);
            e.HasIndex(c => c.Code).IsUnique();

            e.HasMany(c => c.Regions)
             .WithOne(r => r.Country)
             .HasForeignKey(r => r.CountryId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasData(
                new Country
                {
                    Id = UsId, Code = "US", Name = "United States", IsBuiltIn = true, SortOrder = 1,
                    Region1Name = "State", Region1UseList = true, DisplayRegion2 = false
                },
                new Country
                {
                    Id = CaId, Code = "CA", Name = "Canada", IsBuiltIn = true, SortOrder = 2,
                    Region1Name = "Province", Region1UseList = true, DisplayRegion2 = false
                });
        }
    }
}
