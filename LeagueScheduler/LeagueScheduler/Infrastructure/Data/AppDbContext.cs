using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Features.Common.Entities;
using LeagueScheduler.Features.Courts.Entities;
using LeagueScheduler.Features.Leagues.Entities;
using LeagueScheduler.Features.Players.Entities;
using LeagueScheduler.Features.Scheduling.Entities;
using LeagueScheduler.Features.Seasons.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Court> Courts => Set<Court>();
        public DbSet<League> Leagues => Set<League>();
        public DbSet<LeaguePlayer> LeaguePlayers => Set<LeaguePlayer>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Season> Seasons => Set<Season>();
        public DbSet<SeasonCourt> SeasonCourts => Set<SeasonCourt>();
        public DbSet<PrePlannedEvent> PrePlannedEvents => Set<PrePlannedEvent>();
        public DbSet<ScheduleResult> ScheduleResults => Set<ScheduleResult>();
        public DbSet<ScheduleMatch> ScheduleMatches => Set<ScheduleMatch>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            model.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
