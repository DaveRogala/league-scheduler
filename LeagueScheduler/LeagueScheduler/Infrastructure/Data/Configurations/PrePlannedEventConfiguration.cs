using System.Text.Json;
using LeagueScheduler.Features.Seasons.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class PrePlannedEventConfiguration : IEntityTypeConfiguration<PrePlannedEvent>
    {
        private static readonly JsonSerializerOptions JsonOpts = new();

        public void Configure(EntityTypeBuilder<PrePlannedEvent> e)
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.HasOne(p => p.Season)
                .WithMany(s => s.PrePlannedEvents)
                .HasForeignKey(p => p.SeasonId);
            e.HasOne(p => p.Court)
                .WithMany()
                .HasForeignKey(p => p.CourtId)
                .IsRequired(false);
            e.Property(p => p.PlayerIds)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonOpts),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, JsonOpts) ?? new List<Guid>())
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
                    (a, b) => JsonSerializer.Serialize(a, JsonOpts) == JsonSerializer.Serialize(b, JsonOpts),
                    v => JsonSerializer.Serialize(v, JsonOpts).GetHashCode(),
                    v => new List<Guid>(v)));
        }
    }
}
