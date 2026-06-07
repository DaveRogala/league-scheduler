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

        public async Task<ScheduleResultDto> SaveAsync(ScheduleResultDto dto)
        {
            // Do not set Id on entity or matches — the database generates them via uuid_generate_v1mc().
            var entity = new ScheduleResult
            {
                SeasonId = dto.SeasonId,
                FairnessToleranceUsed = dto.FairnessToleranceUsed,
                AssignedCounts = dto.AssignedCounts,
                TargetCounts = dto.TargetCounts,
                Conflicts = [.. dto.Conflicts],
                Matches = dto.Matches.Select(m => new ScheduleMatch
                {
                    Date = m.Date,
                    CourtId = m.CourtId,
                    CourtNumber = 0,
                    PlayerIds = [.. m.PlayerIds]
                }).ToList()
            };

            _db.ScheduleResults.Add(entity);
            await _db.SaveChangesAsync();

            // Return the DTO enriched with the DB-generated IDs.
            return dto with
            {
                Id = entity.Id,
                Matches = [.. dto.Matches.Zip(entity.Matches, (m, e) => m with { Id = e.Id })]
            };
        }

        public async Task<ScheduleResultDto?> LoadAsync(Guid id)
        {
            var entity = await _db.ScheduleResults
                .Include(r => r.Matches)
                    .ThenInclude(m => m.Court)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (entity is null) return null;

            return new ScheduleResultDto
            {
                Id = entity.Id,
                SeasonId = entity.SeasonId,
                FairnessToleranceUsed = entity.FairnessToleranceUsed,
                AssignedCounts = entity.AssignedCounts,
                TargetCounts = entity.TargetCounts,
                Conflicts = [.. entity.Conflicts],
                Matches = entity.Matches.Select(m => new MatchDto
                {
                    Id = m.Id,
                    Date = m.Date,
                    CourtId = m.CourtId ?? Guid.Empty,
                    CourtName = m.Court?.Name ?? string.Empty,
                    PlayerIds = [.. m.PlayerIds]
                }).ToList()
            };
        }
    }
}
