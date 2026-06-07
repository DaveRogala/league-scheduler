using LeagueScheduler.Shared.Users;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Users;

public class PronounsClient(HttpClient http)
{
    public Task<List<SupportedPronounsDto>?> GetAllAsync() =>
        http.GetFromJsonAsync<List<SupportedPronounsDto>>("api/pronouns");

    public Task<HttpResponseMessage> AddAsync(SupportedPronounsDto dto) =>
        http.PostAsJsonAsync("api/admin/pronouns", dto);

    public Task<HttpResponseMessage> DeleteAsync(Guid id) =>
        http.DeleteAsync($"api/admin/pronouns/{id}");
}
