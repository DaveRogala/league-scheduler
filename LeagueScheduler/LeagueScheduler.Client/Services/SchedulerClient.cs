using System.Net.Http.Json;
using System.Threading.Tasks;
using LeagueScheduler.Shared.Models;

namespace LeagueScheduler.Client.Services
{
    public class SchedulerClient
    {
        private readonly HttpClient _http;

        public SchedulerClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ScheduleResultDto?> ComputeAsync(ScheduleRequestDto request)
        {
            var resp = await _http.PostAsJsonAsync("/api/scheduler/compute", request);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadFromJsonAsync<ScheduleResultDto>();
        }
    }
}
