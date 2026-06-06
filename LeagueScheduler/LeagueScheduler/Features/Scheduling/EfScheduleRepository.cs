using LeagueScheduler.Features.Scheduling.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Scheduling;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Scheduling
{
    public class EfScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _db;

        public EfScheduleRepository(AppDbContext db) => _db = db;

        public async Task SaveAsync(ScheduleResultDto dto)
        {
            var entity = new ScheduleResult
            {
                Id = dto.Id,
                FairnessToleranceUsed = dto.FairnessToleranceUsed,
                AssignedCounts = dto.AssignedCounts,
                TargetCounts = dto.TargetCounts,
                Conflicts = [.. dto.Conflicts],
                Matches = dto.Matches.Select(m => new ScheduleMatch
                {
                    Id = m.Id,
                    Date = m.Date,
                    Court = m.Court,
                    PlayerIds = [.. m.PlayerIds]
                }).ToList()
            };

            _db.ScheduleResults.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<ScheduleResultDto?> LoadAsync(Guid id)
        {
            var entity = await _db.ScheduleResults
                .Include(r => r.Matches)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (entity is null) return null;

            return new ScheduleResultDto
            {
                Id = entity.Id,
                FairnessToleranceUsed = entity.FairnessToleranceUsed,
                AssignedCounts = entity.AssignedCounts,
                TargetCounts = entity.TargetCounts,
                Conflicts = [.. entity.Conflicts],
                Matches = entity.Matches.Select(m => new MatchDto
                {
                    Id = m.Id,
                    Date = m.Date,
                    Court = m.Court,
                    PlayerIds = [.. m.PlayerIds]
                }).ToList()
            };
        }
    }
}
