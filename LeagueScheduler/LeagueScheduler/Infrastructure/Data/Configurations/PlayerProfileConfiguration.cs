using LeagueScheduler.Features.Players.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class PlayerProfileConfiguration : IEntityTypeConfiguration<PlayerProfile>
    {
        public void Configure(EntityTypeBuilder<PlayerProfile> e)
        {
            e.HasKey(p => p.UserId);

            e.Property(p => p.PhoneNumber).HasMaxLength(30);
            e.Property(p => p.GenderDescription).HasMaxLength(100);
            e.Property(p => p.NtrpRating).HasPrecision(3, 1);
            e.Property(p => p.WtnSingles).HasPrecision(5, 2);
            e.Property(p => p.WtnDoubles).HasPrecision(5, 2);
            e.Property(p => p.UtrRating).HasPrecision(5, 2);

            // One-to-one with AppUser; cascades when the user account is deleted.
            e.HasOne(p => p.User)
             .WithOne()
             .HasForeignKey<PlayerProfile>(p => p.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            // Optional address; NULLed out if the Address row is deleted independently.
            e.HasOne(p => p.Address)
             .WithMany()
             .HasForeignKey(p => p.AddressId)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
