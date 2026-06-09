using LeagueScheduler.Shared.Players;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Players;

public class PlayerProfileClient(HttpClient http)
{
    public async Task<PlayerProfileDto?> GetAsync() =>
        await http.GetFromJsonAsync<PlayerProfileDto>("/api/player-profile");

    public async Task<HttpResponseMessage> UpdateAsync(PlayerProfileDto dto) =>
        await http.PutAsJsonAsync("/api/player-profile", dto);
}
