using System.Text.Json;
using LeagueScheduler.Features.Scheduling.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private static readonly JsonSerializerOptions JsonOpts = new();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ScheduleResult> ScheduleResults => Set<ScheduleResult>();
        public DbSet<ScheduleMatch> ScheduleMatches => Set<ScheduleMatch>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<ScheduleResult>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.AssignedCounts)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonOpts),
                        v => JsonSerializer.Deserialize<Dictionary<Guid, int>>(v, JsonOpts) ?? new())
                    .HasColumnType("jsonb");
                e.Property(r => r.TargetCounts)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonOpts),
                        v => JsonSerializer.Deserialize<Dictionary<Guid, int>>(v, JsonOpts) ?? new())
                    .HasColumnType("jsonb");
                e.Property(r => r.Conflicts)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonOpts),
                        v => JsonSerializer.Deserialize<List<string>>(v, JsonOpts) ?? new())
                    .HasColumnType("jsonb");
                e.HasMany(r => r.Matches)
                    .WithOne(m => m.ScheduleResult)
                    .HasForeignKey(m => m.ScheduleResultId);
            });

            model.Entity<ScheduleMatch>(e =>
            {
                e.HasKey(m => m.Id);
                e.Property(m => m.PlayerIds)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonOpts),
                        v => JsonSerializer.Deserialize<List<Guid>>(v, JsonOpts) ?? new())
                    .HasColumnType("jsonb");
            });
        }
    }
}
