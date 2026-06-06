using LeagueScheduler.Features.Courts.Entities;
using LeagueScheduler.Features.Leagues.Entities;
using LeagueScheduler.Features.Players.Entities;
using LeagueScheduler.Features.Scheduling.Entities;
using LeagueScheduler.Features.Seasons.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Court> Courts => Set<Court>();
        public DbSet<League> Leagues => Set<League>();
        public DbSet<LeaguePlayer> LeaguePlayers => Set<LeaguePlayer>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Season> Seasons => Set<Season>();
        public DbSet<SeasonCourt> SeasonCourts => Set<SeasonCourt>();
        public DbSet<PrePlannedEvent> PrePlannedEvents => Set<PrePlannedEvent>();
        public DbSet<ScheduleResult> ScheduleResults => Set<ScheduleResult>();
        public DbSet<ScheduleMatch> ScheduleMatches => Set<ScheduleMatch>();

        protected override void OnModelCreating(ModelBuilder model) =>
            model.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
