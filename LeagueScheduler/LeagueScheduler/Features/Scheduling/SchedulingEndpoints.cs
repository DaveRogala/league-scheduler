using LeagueScheduler.Shared.Scheduling;

namespace LeagueScheduler.Features.Scheduling
{
    public static class SchedulingEndpoints
    {
        public static void MapSchedulingEndpoints(this WebApplication app)
        {
            app.MapPost("/api/scheduler/compute", async (ScheduleRequestDto request, ISchedulerService scheduler) =>
            {
                var result = await scheduler.ScheduleAsync(request);
                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}
