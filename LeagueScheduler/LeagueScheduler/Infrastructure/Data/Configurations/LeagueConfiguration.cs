using LeagueScheduler.Features.Leagues.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class LeagueConfiguration : AuditableEntityConfiguration<League>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<League> e)
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(l => l.Name).IsRequired().HasMaxLength(200);
            e.Property(l => l.Mode).HasConversion<string>();

            e.HasOne(l => l.MatchType)
             .WithMany()
             .HasForeignKey(l => l.MatchTypeId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
