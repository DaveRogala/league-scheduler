using MudBlazor.Services;
using LeagueScheduler.Client.Pages;
using LeagueScheduler.Components;
using LeagueScheduler.Services;
using LeagueScheduler.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Scheduler service
builder.Services.Configure<ScheduleOptions>(builder.Configuration.GetSection("Scheduling"));
builder.Services.AddSingleton<IScheduleRepository, JsonScheduleRepository>();
builder.Services.AddSingleton<ISchedulerService, SchedulerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LeagueScheduler.Client._Imports).Assembly);

// Minimal API endpoint for scheduling
app.MapPost("/api/scheduler/compute", async (ScheduleRequestDto request, ISchedulerService scheduler) =>
{
    var result = await scheduler.ScheduleAsync(request);
    return Results.Ok(result);
});

app.Run();
