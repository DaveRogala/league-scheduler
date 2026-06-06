using System.Threading.Tasks;
using LeagueScheduler.Shared.Models;

namespace LeagueScheduler.Services
{
    public interface ISchedulerService
    {
        Task<ScheduleResultDto> ScheduleAsync(ScheduleRequestDto request);
    }
}
