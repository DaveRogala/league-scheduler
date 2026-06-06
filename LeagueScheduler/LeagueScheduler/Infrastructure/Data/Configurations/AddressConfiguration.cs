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
            e.Property(a => a.Line1).HasMaxLength(200);
            e.Property(a => a.Line2).HasMaxLength(200);
            e.Property(a => a.Line3).HasMaxLength(200);
            e.Property(a => a.Locality).HasMaxLength(100);
            e.Property(a => a.AdminArea).HasMaxLength(100);
            e.Property(a => a.SubAdminArea).HasMaxLength(100);
            e.Property(a => a.PostalCode).HasMaxLength(20);
            e.Property(a => a.CountryCode).HasMaxLength(2);
            e.Property(a => a.VisibleFields).HasConversion<int>();
        }
    }
}
