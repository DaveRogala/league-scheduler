using LeagueScheduler.Features.Courts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations;

public class CourtHistoryConfiguration : IEntityTypeConfiguration<CourtHistory>
{
    public void Configure(EntityTypeBuilder<CourtHistory> e)
    {
        e.ToTable("CourtHistory");
        e.HasKey(h => h.Id);
        e.Property(h => h.Id).HasDefaultValueSql("uuid_generate_v1mc()");
        e.Property(h => h.Operation).IsRequired().HasMaxLength(10);
        e.Property(h => h.Name).IsRequired().HasMaxLength(200);
        e.Property(h => h.Type).HasConversion<string>();
    }
}
