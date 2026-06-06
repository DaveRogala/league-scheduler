using System.Text.Json;
using LeagueScheduler.Features.Scheduling.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class ScheduleResultConfiguration : IEntityTypeConfiguration<ScheduleResult>
    {
        private static readonly JsonSerializerOptions JsonOpts = new();

        public void Configure(EntityTypeBuilder<ScheduleResult> e)
        {
            e.HasKey(r => r.Id);
            e.HasOne(r => r.Season)
                .WithMany(s => s.ScheduleResults)
                .HasForeignKey(r => r.SeasonId)
                .IsRequired(false);
            e.Property(r => r.AssignedCounts)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonOpts),
                    v => JsonSerializer.Deserialize<Dictionary<Guid, int>>(v, JsonOpts) ?? new())
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<Guid, int>>(
                    (a, b) => JsonSerializer.Serialize(a, JsonOpts) == JsonSerializer.Serialize(b, JsonOpts),
                    v => JsonSerializer.Serialize(v, JsonOpts).GetHashCode(),
                    v => JsonSerializer.Deserialize<Dictionary<Guid, int>>(JsonSerializer.Serialize(v, JsonOpts), JsonOpts)!));
            e.Property(r => r.TargetCounts)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonOpts),
                    v => JsonSerializer.Deserialize<Dictionary<Guid, int>>(v, JsonOpts) ?? new())
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<Guid, int>>(
                    (a, b) => JsonSerializer.Serialize(a, JsonOpts) == JsonSerializer.Serialize(b, JsonOpts),
                    v => JsonSerializer.Serialize(v, JsonOpts).GetHashCode(),
                    v => JsonSerializer.Deserialize<Dictionary<Guid, int>>(JsonSerializer.Serialize(v, JsonOpts), JsonOpts)!));
            e.Property(r => r.Conflicts)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonOpts),
                    v => JsonSerializer.Deserialize<List<string>>(v, JsonOpts) ?? new List<string>())
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (a, b) => JsonSerializer.Serialize(a, JsonOpts) == JsonSerializer.Serialize(b, JsonOpts),
                    v => JsonSerializer.Serialize(v, JsonOpts).GetHashCode(),
                    v => new List<string>(v)));
            e.HasMany(r => r.Matches)
                .WithOne(m => m.ScheduleResult)
                .HasForeignKey(m => m.ScheduleResultId);
        }
    }
}
