using LeagueScheduler.Shared.Courts;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Courts
{
    public class CourtClient(HttpClient http)
    {
        public Task<List<CourtDto>?> GetAllAsync() =>
            http.GetFromJsonAsync<List<CourtDto>>("api/courts");

        public Task<HttpResponseMessage> CreateAsync(CourtDto dto) =>
            http.PostAsJsonAsync("api/courts", dto);

        public Task<HttpResponseMessage> UpdateAsync(Guid id, CourtDto dto) =>
            http.PutAsJsonAsync($"api/courts/{id}", dto);

        public Task<HttpResponseMessage> DeleteAsync(Guid id) =>
            http.DeleteAsync($"api/courts/{id}");
    }
}
