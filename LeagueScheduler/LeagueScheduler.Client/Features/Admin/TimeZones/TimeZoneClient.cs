using LeagueScheduler.Shared.Admin;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Admin.TimeZones
{
    public class TimeZoneClient(HttpClient http)
    {
        public Task<List<SupportedTimeZoneDto>?> GetEnabledAsync() =>
            http.GetFromJsonAsync<List<SupportedTimeZoneDto>>("api/timezones");

        public Task<List<SupportedTimeZoneDto>?> GetAllAsync() =>
            http.GetFromJsonAsync<List<SupportedTimeZoneDto>>("api/admin/timezones");

        public Task<HttpResponseMessage> UpdateAsync(SupportedTimeZoneDto dto) =>
            http.PutAsJsonAsync($"api/admin/timezones/{dto.Id}", dto);
    }
}
