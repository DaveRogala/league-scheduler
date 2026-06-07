using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Features.Courts.Entities;
using LeagueScheduler.Infrastructure.Data;
using LeagueScheduler.Shared.Courts;
using Microsoft.EntityFrameworkCore;

namespace LeagueScheduler.Features.Courts
{
    public static class CourtHistoryEndpoints
    {
        public static IEndpointRouteBuilder MapCourtHistoryEndpoints(this IEndpointRouteBuilder app)
        {
            var g = app.MapGroup("/api/courts/{courtId:guid}/history").RequireAuthorization();

            g.MapGet("", async (
                Guid courtId,
                AppDbContext db,
                int page = 1,
                int pageSize = 50) =>
            {
                var query = db.CourtHistories
                    .Where(h => h.CourtId == courtId)
                    .OrderByDescending(h => h.ChangedAt);

                var total = await query.CountAsync();

                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userIds = items
                    .Where(h => h.ChangedById.HasValue)
                    .Select(h => h.ChangedById!.Value)
                    .Distinct()
                    .ToList();

                var userNames = userIds.Count > 0
                    ? await db.Users
                        .IgnoreQueryFilters()
                        .Where(u => userIds.Contains(u.Id))
                        .ToDictionaryAsync(u => u.Id, u => u.DisplayName)
                    : [];

                var dtos = items.Select(h => ToDto(h, userNames)).ToList();
                return Results.Ok(new CourtHistoryPageDto { Items = dtos, TotalCount = total });
            });

            g.MapPost("{historyId:guid}/revert", async (
                Guid courtId,
                Guid historyId,
                AppDbContext db) =>
            {
                var snapshot = await db.CourtHistories
                    .FirstOrDefaultAsync(h => h.Id == historyId && h.CourtId == courtId);
                if (snapshot is null) return Results.NotFound();

                var court = await db.Courts
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(c => c.Id == courtId);
                if (court is null) return Results.NotFound();

                court.Name = snapshot.Name;
                court.Type = snapshot.Type;
                court.NumberOfCourts = snapshot.NumberOfCourts;
                court.Lighted = snapshot.Lighted;
                court.HoursOfOperation = snapshot.HoursOfOperation;
                court.AccessNotes = snapshot.AccessNotes;
                court.Description = snapshot.Description;
                court.Condition = snapshot.Condition;
                court.AddressId = snapshot.AddressId;
                court.DeletedAt = null;
                court.DeletedById = null;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }

        private static CourtHistoryDto ToDto(CourtHistory h, Dictionary<Guid, string> userNames) => new()
        {
            Id = h.Id,
            CourtId = h.CourtId,
            Operation = h.Operation,
            ChangedAt = h.ChangedAt,
            ChangedById = h.ChangedById,
            ChangedByName = h.ChangedById.HasValue && userNames.TryGetValue(h.ChangedById.Value, out var name)
                ? name : null,
            Name = h.Name,
            Type = h.Type,
            NumberOfCourts = h.NumberOfCourts,
            Lighted = h.Lighted,
            HoursOfOperation = h.HoursOfOperation,
            AccessNotes = h.AccessNotes,
            Description = h.Description,
            Condition = h.Condition,
            AddressId = h.AddressId,
        };
    }
}
