using LeagueScheduler.Features.Admin.Logs.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Admin;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Admin.Logs
{
    public static class LogEndpoints
    {
        public static IEndpointRouteBuilder MapLogEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/admin/logs").RequireAuthorization();

            g.MapGet("", async (
                AppDbContext db,
                string? level,
                string? search,
                DateTimeOffset? from,
                DateTimeOffset? to,
                int page = 1,
                int pageSize = 50) =>
            {
                var query = BuildQuery(db.Logs, level, search, from, to);
                int total = await query.CountAsync();
                var items = await query
                    .OrderByDescending(l => l.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(l => ToDto(l))
                    .ToListAsync();

                return Results.Ok(new LogPageDto { Items = items, TotalCount = total });
            });

            g.MapDelete("{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var entry = await db.Logs.FindAsync(id);
                if (entry is null) return Results.NotFound();
                db.Logs.Remove(entry);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // Purge entries older than the given number of days (default 90).
            g.MapDelete("purge", async (AppDbContext db, int days = 90) =>
            {
                var cutoff = DateTimeOffset.UtcNow.AddDays(-days);
                var deleted = await db.Logs
                    .Where(l => l.Timestamp < cutoff)
                    .ExecuteDeleteAsync();
                return Results.Ok(new { deleted });
            });

            return app;
        }

        private static IQueryable<LogEntry> BuildQuery(
            IQueryable<LogEntry> source,
            string? level,
            string? search,
            DateTimeOffset? from,
            DateTimeOffset? to)
        {
            if (!string.IsNullOrWhiteSpace(level))
                source = source.Where(l => l.Level == level);

            if (from.HasValue)
                source = source.Where(l => l.Timestamp >= from.Value);

            if (to.HasValue)
                source = source.Where(l => l.Timestamp <= to.Value);

            // Full-text search across message, exception, and structured properties.
            if (!string.IsNullOrWhiteSpace(search))
                source = source.Where(l =>
                    l.Message.Contains(search) ||
                    (l.Exception != null && l.Exception.Contains(search)) ||
                    (l.Properties != null && l.Properties.Contains(search)));

            return source;
        }

        private static LogEntryDto ToDto(LogEntry l) => new()
        {
            Id = l.Id,
            Timestamp = l.Timestamp,
            Level = l.Level,
            Message = l.Message,
            Exception = l.Exception,
            Properties = l.Properties
        };
    }
}
