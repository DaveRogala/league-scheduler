using System.Text.Json;
using LeagueScheduler.Shared.Scheduling;
using Microsoft.Extensions.Options;

namespace LeagueScheduler.Features.Scheduling
{
    public class JsonScheduleRepository : IScheduleRepository
    {
        private readonly string _dataFolder;
        private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

        public JsonScheduleRepository(IOptions<ScheduleOptions> opts)
        {
            var folder = opts.Value.DataFolder ?? "Data";
            _dataFolder = Path.GetFullPath(folder);
            if (!Directory.Exists(_dataFolder)) Directory.CreateDirectory(_dataFolder);
        }

        public async Task SaveAsync(ScheduleResultDto result)
        {
            var path = Path.Combine(_dataFolder, $"schedule_{result.Id}.json");
            using var fs = File.Create(path);
            await JsonSerializer.SerializeAsync(fs, result, _opts);
        }

        public async Task<ScheduleResultDto?> LoadAsync(Guid id)
        {
            var path = Path.Combine(_dataFolder, $"schedule_{id}.json");
            if (!File.Exists(path)) return null;
            using var fs = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<ScheduleResultDto>(fs);
        }
    }
}
