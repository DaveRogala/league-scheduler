using System.Text.Json;
using LeagueScheduler.Features.Players.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueScheduler.Infrastructure.Data.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        private static readonly JsonSerializerOptions JsonOpts = new();

        public void Configure(EntityTypeBuilder<Player> e)
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Role).HasConversion<string>();
            e.Property(p => p.Nudge).HasConversion<string>();
            e.Property(p => p.UnavailableDates)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonOpts),
                    v => JsonSerializer.Deserialize<List<DateTime>>(v, JsonOpts) ?? new List<DateTime>())
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<List<DateTime>>(
                    (a, b) => JsonSerializer.Serialize(a, JsonOpts) == JsonSerializer.Serialize(b, JsonOpts),
                    v => JsonSerializer.Serialize(v, JsonOpts).GetHashCode(),
                    v => JsonSerializer.Deserialize<List<DateTime>>(JsonSerializer.Serialize(v, JsonOpts), JsonOpts)!));
        }
    }
}
