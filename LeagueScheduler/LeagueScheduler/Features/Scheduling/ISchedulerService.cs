using LeagueScheduler.Shared.Scheduling;

namespace LeagueScheduler.Features.Scheduling
{
    public interface ISchedulerService
    {
        Task<ScheduleResultDto> ScheduleAsync(ScheduleRequestDto request);
    }
}
