using LeagueScheduler.Shared.Courts;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Courts
{
    public class CourtHistoryClient(HttpClient http)
    {
        public Task<CourtHistoryPageDto?> GetPageAsync(Guid courtId, int page = 1, int pageSize = 50) =>
            http.GetFromJsonAsync<CourtHistoryPageDto>(
                $"api/courts/{courtId}/history?page={page}&pageSize={pageSize}");

        public Task<HttpResponseMessage> RevertAsync(Guid courtId, Guid historyId) =>
            http.PostAsync($"api/courts/{courtId}/history/{historyId}/revert", null);
    }
}
