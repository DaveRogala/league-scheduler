using LeagueScheduler.Features.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> e)
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(a => a.Line1).HasMaxLength(200);
            e.Property(a => a.Line2).HasMaxLength(200);
            e.Property(a => a.Line3).HasMaxLength(200);
            e.Property(a => a.Locality).HasMaxLength(100);
            e.Property(a => a.AdminArea).HasMaxLength(100);
            e.Property(a => a.SubAdminArea).HasMaxLength(100);
            e.Property(a => a.PostalCode).HasMaxLength(20);
            e.Property(a => a.VisibleFields).HasConversion<int>();

            e.HasOne(a => a.Country)
             .WithMany()
             .HasForeignKey(a => a.CountryId)
             .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(a => a.AdminAreaRegion)
             .WithMany()
             .HasForeignKey(a => a.AdminAreaId)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
