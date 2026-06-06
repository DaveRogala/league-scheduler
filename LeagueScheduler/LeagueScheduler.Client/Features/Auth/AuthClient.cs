using LeagueScheduler.Shared.Auth;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Auth
{
    public class AuthClient
    {
        private readonly HttpClient _http;

        public AuthClient(HttpClient http) => _http = http;

        public async Task<AuthResultDto?> LoginAsync(LoginRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", dto);
            return await response.Content.ReadFromJsonAsync<AuthResultDto>();
        }

        public async Task<AuthResultDto?> RegisterAsync(RegisterRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/register", dto);
            return await response.Content.ReadFromJsonAsync<AuthResultDto>();
        }
    }
}
