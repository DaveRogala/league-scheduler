using LeagueScheduler.Shared.Leagues;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Leagues
{
    public class LeagueClient(HttpClient http)
    {
        public Task<List<LeagueDto>?> GetAllAsync() =>
            http.GetFromJsonAsync<List<LeagueDto>>("api/leagues");

        public Task<HttpResponseMessage> CreateAsync(LeagueDto dto) =>
            http.PostAsJsonAsync("api/leagues", dto);

        public Task<HttpResponseMessage> UpdateAsync(Guid id, LeagueDto dto) =>
            http.PutAsJsonAsync($"api/leagues/{id}", dto);

        public Task<HttpResponseMessage> DeleteAsync(Guid id) =>
            http.DeleteAsync($"api/leagues/{id}");
    }
}
