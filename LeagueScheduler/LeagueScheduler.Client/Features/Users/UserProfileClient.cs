using LeagueScheduler.Shared.Users;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Users
{
    public class UserProfileClient(HttpClient http)
    {
        public Task<UserProfileDto?> GetAsync() =>
            http.GetFromJsonAsync<UserProfileDto>("api/profile");

        public Task<HttpResponseMessage> UpdateAsync(UserProfileDto dto) =>
            http.PutAsJsonAsync("api/profile", dto);
    }
}
