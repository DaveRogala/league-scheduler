using LeagueScheduler.Shared.Scheduling;

namespace LeagueScheduler.Features.Scheduling
{
    public static class SchedulingEndpoints
    {
        public static void MapSchedulingEndpoints(this WebApplication app)
        {
            app.MapPost("/api/scheduler/compute", async (
                ScheduleRequestDto request,
                ISchedulerService scheduler,
                ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SchedulingEndpoints));

                logger.LogInformation(
                    "Schedule compute requested for season {SeasonId} ({MatchType}, {PlayerCount} players)",
                    request.Season.Id, request.Season.MatchType, request.Players.Count);

                var result = await scheduler.ScheduleAsync(request);

                if (result.Conflicts.Count > 0)
                    logger.LogWarning(
                        "Schedule {ScheduleId} for season {SeasonId} produced {ConflictCount} conflict(s): {Conflicts}",
                        result.Id, result.SeasonId, result.Conflicts.Count, string.Join("; ", result.Conflicts));
                else
                    logger.LogInformation(
                        "Schedule {ScheduleId} generated {MatchCount} matches for season {SeasonId}",
                        result.Id, result.Matches.Count, result.SeasonId);

                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}
