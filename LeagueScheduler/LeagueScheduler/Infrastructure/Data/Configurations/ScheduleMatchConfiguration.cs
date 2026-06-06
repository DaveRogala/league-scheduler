using System.Text.Json;
using LeagueScheduler.Features.Scheduling.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class ScheduleMatchConfiguration : IEntityTypeConfiguration<ScheduleMatch>
    {
        private static readonly JsonSerializerOptions JsonOpts = new();

        public void Configure(EntityTypeBuilder<ScheduleMatch> e)
        {
            e.HasKey(m => m.Id);
            // Map CourtNumber to the existing "Court" column from the InitialCreate migration
            e.Property(m => m.CourtNumber).HasColumnName("Court");
            e.HasOne(m => m.Court)
                .WithMany()
                .HasForeignKey(m => m.CourtId)
                .IsRequired(false);
            e.Property(m => m.PlayerIds)
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
