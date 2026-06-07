using System.Text.Json;
using LeagueScheduler.Features.Seasons.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class SeasonConfiguration : AuditableEntityConfiguration<Season>
    {
        private static readonly JsonSerializerOptions JsonOpts = new();

        protected override void ConfigureEntity(EntityTypeBuilder<Season> e)
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).HasDefaultValueSql("uuid_generate_v1mc()");
            e.Property(s => s.Name).IsRequired().HasMaxLength(200);
            e.HasOne(s => s.League)
                .WithMany(l => l.Seasons)
                .HasForeignKey(s => s.LeagueId);
            e.Property(s => s.DaysOfWeek)
                .HasConversion(
                    v => JsonSerializer.Serialize(v.Select(d => (int)d).ToList(), JsonOpts),
                    v => JsonSerializer.Deserialize<List<int>>(v, JsonOpts)!.Select(i => (DayOfWeek)i).ToList())
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<List<DayOfWeek>>(
                    (a, b) => JsonSerializer.Serialize(a, JsonOpts) == JsonSerializer.Serialize(b, JsonOpts),
                    v => JsonSerializer.Serialize(v, JsonOpts).GetHashCode(),
                    v => new List<DayOfWeek>(v)));
            e.Property(s => s.NonPlayDates)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonOpts),
                    v => JsonSerializer.Deserialize<List<DateTime>>(v, JsonOpts) ?? new List<DateTime>())
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<List<DateTime>>(
                    (a, b) => JsonSerializer.Serialize(a, JsonOpts) == JsonSerializer.Serialize(b, JsonOpts),
                    v => JsonSerializer.Serialize(v, JsonOpts).GetHashCode(),
                    v => new List<DateTime>(v)));
        }
    }
}
