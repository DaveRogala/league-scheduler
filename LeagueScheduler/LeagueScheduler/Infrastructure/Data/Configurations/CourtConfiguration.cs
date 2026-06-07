using LeagueScheduler.Features.Courts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class CourtConfiguration : IEntityTypeConfiguration<Court>
    {
        public void Configure(EntityTypeBuilder<Court> e)
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(c => c.Name).IsRequired().HasMaxLength(200);
            e.Property(c => c.Type).HasConversion<string>();
            e.HasOne(c => c.Address)
             .WithMany()
             .HasForeignKey(c => c.AddressId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
