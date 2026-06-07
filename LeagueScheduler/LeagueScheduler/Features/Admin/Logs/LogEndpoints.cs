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
                bool caseSensitive = false,
                string? sortBy = null,
                bool sortAsc = false,
                int page = 1,
                int pageSize = 50) =>
            {
                var query = BuildQuery(db.Logs, level, search, from, to, caseSensitive);
                int total = await query.CountAsync();

                IOrderedQueryable<LogEntry> ordered = (sortBy, sortAsc) switch
                {
                    ("level", true)  => query.OrderBy(l => l.Level),
                    ("level", false) => query.OrderByDescending(l => l.Level),
                    (_, true)        => query.OrderBy(l => l.Timestamp),
                    _                => query.OrderByDescending(l => l.Timestamp)
                };

                var items = await ordered
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
            DateTimeOffset? to,
            bool caseSensitive = false)
        {
            if (!string.IsNullOrWhiteSpace(level))
                source = source.Where(l => l.Level == level);

            if (from.HasValue)
                source = source.Where(l => l.Timestamp >= from.Value);

            if (to.HasValue)
                source = source.Where(l => l.Timestamp <= to.Value);

            // Search message and exception text. Properties is jsonb and doesn't support
            // LIKE in PostgreSQL without an explicit ::text cast; omit it from LINQ search.
            // * is mapped to % for user-friendly wildcards; bare terms are auto-wrapped in %.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var pattern = ToLikePattern(search);
                source = caseSensitive
                    ? source.Where(l =>
                        EF.Functions.Like(l.Message, pattern) ||
                        (l.Exception != null && EF.Functions.Like(l.Exception, pattern)))
                    : source.Where(l =>
                        EF.Functions.ILike(l.Message, pattern) ||
                        (l.Exception != null && EF.Functions.ILike(l.Exception, pattern)));
            }

            return source;
        }

        // Maps * to % for user-friendly wildcards; always wraps in % so wildcards work as
        // sub-string matchers, not full-string anchors. Use % directly for anchored patterns.
        private static string ToLikePattern(string search)
            => $"%{search.Replace("*", "%")}%";

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
