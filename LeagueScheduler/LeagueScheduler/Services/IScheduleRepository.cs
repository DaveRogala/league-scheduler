using System.Threading.Tasks;
using LeagueScheduler.Shared.Models;

namespace LeagueScheduler.Services
{
    public interface IScheduleRepository
    {
        Task SaveAsync(ScheduleResultDto result);
        Task<ScheduleResultDto?> LoadAsync(System.Guid id);
    }
}
