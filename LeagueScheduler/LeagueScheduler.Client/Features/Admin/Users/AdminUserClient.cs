using LeagueScheduler.Shared.Admin.Users;
using LeagueScheduler.Shared.Auth;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Admin.Users;

public class AdminUserClient(HttpClient http)
{
    public async Task<AdminUserListDto?> GetUsersAsync(string? search = null, int page = 1, int pageSize = 20)
    {
        var query = BuildQuery(search, page, pageSize);
        return await http.GetFromJsonAsync<AdminUserListDto>($"/api/admin/users?{query}");
    }

    public async Task<HttpResponseMessage> ResetPasswordAsync(Guid userId, AdminResetPasswordDto dto) =>
        await http.PostAsJsonAsync($"/api/admin/users/{userId}/reset-password", dto);

    public async Task<HttpResponseMessage> DisableAsync(Guid userId) =>
        await http.PostAsync($"/api/admin/users/{userId}/disable", null);

    public async Task<HttpResponseMessage> EnableAsync(Guid userId) =>
        await http.PostAsync($"/api/admin/users/{userId}/enable", null);

    public async Task<AuthResultDto?> ImpersonateAsync(Guid userId)
    {
        var response = await http.PostAsync($"/api/admin/users/{userId}/impersonate", null);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<AuthResultDto>()
            : null;
    }

    private static string BuildQuery(string? search, int page, int pageSize)
    {
        var parts = new List<string> { $"page={page}", $"pageSize={pageSize}" };
        if (!string.IsNullOrWhiteSpace(search))
            parts.Add($"search={Uri.EscapeDataString(search)}");
        return string.Join("&", parts);
    }
}
