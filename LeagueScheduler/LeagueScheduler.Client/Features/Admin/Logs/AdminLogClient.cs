using LeagueScheduler.Shared.Admin;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Admin.Logs
{
    public class AdminLogClient(HttpClient http)
    {
        public async Task<LogPageDto?> GetPageAsync(
            string? level, string? search, DateTimeOffset? from, DateTimeOffset? to,
            int page, int pageSize)
        {
            var query = BuildQuery(level, search, from, to, page, pageSize);
            return await http.GetFromJsonAsync<LogPageDto>($"/api/admin/logs?{query}");
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await http.DeleteAsync($"/api/admin/logs/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<int?> PurgeAsync(int days)
        {
            var response = await http.DeleteAsync($"/api/admin/logs/purge?days={days}");
            if (!response.IsSuccessStatusCode) return null;
            var body = await response.Content.ReadFromJsonAsync<PurgeResult>();
            return body?.Deleted;
        }

        private static string BuildQuery(
            string? level, string? search, DateTimeOffset? from, DateTimeOffset? to,
            int page, int pageSize)
        {
            var parts = new List<string>
            {
                $"page={page}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(level))
                parts.Add($"level={Uri.EscapeDataString(level)}");
            if (!string.IsNullOrWhiteSpace(search))
                parts.Add($"search={Uri.EscapeDataString(search)}");
            if (from.HasValue)
                parts.Add($"from={Uri.EscapeDataString(from.Value.ToString("O"))}");
            if (to.HasValue)
                parts.Add($"to={Uri.EscapeDataString(to.Value.ToString("O"))}");

            return string.Join("&", parts);
        }

        private record PurgeResult(int Deleted);
    }
}
