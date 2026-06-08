using LeagueScheduler.Shared.MatchTypes;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.MatchTypes
{
    public class MatchTypeClient(HttpClient http)
    {
        public Task<List<MatchTypeDto>?> GetAllAsync() =>
            http.GetFromJsonAsync<List<MatchTypeDto>>("api/match-types");

        public Task<HttpResponseMessage> CreateAsync(MatchTypeDto dto) =>
            http.PostAsJsonAsync("api/admin/match-types", dto);

        public Task<HttpResponseMessage> UpdateAsync(Guid id, MatchTypeDto dto) =>
            http.PutAsJsonAsync($"api/admin/match-types/{id}", dto);

        public Task<HttpResponseMessage> DeleteAsync(Guid id) =>
            http.DeleteAsync($"api/admin/match-types/{id}");
    }
}
