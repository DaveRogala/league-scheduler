using MudBlazor.Services;
using LeagueScheduler.Client.Features.Scheduling;
using LeagueScheduler.Components;
using LeagueScheduler.Features.Scheduling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.Configure<ScheduleOptions>(builder.Configuration.GetSection("Scheduling"));
builder.Services.AddSingleton<IScheduleRepository, JsonScheduleRepository>();
builder.Services.AddSingleton<ISchedulerService, SchedulerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LeagueScheduler.Client._Imports).Assembly);

app.MapSchedulingEndpoints();

app.Run();
