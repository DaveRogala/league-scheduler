using LeagueScheduler.Features.Admin.Logs.Entities;
using LeagueScheduler.Features.Admin.TimeZones.Entities;
using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Features.Common.Entities;
using LeagueScheduler.Features.Courts.Entities;
using LeagueScheduler.Features.Leagues.Entities;
using LeagueScheduler.Features.SeasonPlayers.Entities;
using LeagueScheduler.Features.Scheduling.Entities;
using LeagueScheduler.Features.Seasons.Entities;
using LeagueScheduler.Infrastructure.Audit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        private readonly ICurrentUserService _currentUserService;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Court> Courts => Set<Court>();
        public DbSet<CourtHistory> CourtHistories => Set<CourtHistory>();
        public DbSet<League> Leagues => Set<League>();
        public DbSet<LeaguePlayer> LeaguePlayers => Set<LeaguePlayer>();
        public DbSet<SeasonPlayer> SeasonPlayers => Set<SeasonPlayer>();
        public DbSet<Season> Seasons => Set<Season>();
        public DbSet<SeasonCourt> SeasonCourts => Set<SeasonCourt>();
        public DbSet<PrePlannedEvent> PrePlannedEvents => Set<PrePlannedEvent>();
        public DbSet<ScheduleResult> ScheduleResults => Set<ScheduleResult>();
        public DbSet<ScheduleMatch> ScheduleMatches => Set<ScheduleMatch>();
        public DbSet<SupportedTimeZone> SupportedTimeZones => Set<SupportedTimeZone>();
        public DbSet<LogEntry> Logs => Set<LogEntry>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            model.HasPostgresExtension("uuid-ossp");
            model.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            model.Entity<AppUser>().HasQueryFilter(u => u.DeletedAt == null);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedById = userId;
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedById = userId;
                        break;
                    case EntityState.Modified:
                        entry.Property(e => e.CreatedAt).IsModified = false;
                        entry.Property(e => e.CreatedById).IsModified = false;
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedById = userId;
                        break;
                }
            }

            // Intercept hard deletes of auditable entities and convert to soft deletes.
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>()
                .Where(e => e.State == EntityState.Deleted)
                .ToList())
            {
                entry.State = EntityState.Modified;
                entry.Entity.DeletedAt = now;
                entry.Entity.DeletedById = userId;
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedById = userId;
            }

            // Soft delete for AppUser.
            foreach (var entry in ChangeTracker.Entries<AppUser>()
                .Where(e => e.State == EntityState.Deleted)
                .ToList())
            {
                entry.State = EntityState.Modified;
                entry.Entity.DeletedAt = now;
                entry.Entity.DeletedById = userId;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
