using LeagueScheduler.Shared.Scheduling;

namespace LeagueScheduler.Features.Scheduling
{
    public interface IScheduleRepository
    {
        Task SaveAsync(ScheduleResultDto result);
        Task<ScheduleResultDto?> LoadAsync(Guid id);
    }
}
