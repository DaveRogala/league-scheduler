using MudBlazor.Services;
using LeagueScheduler.Client.Features.Scheduling;
using LeagueScheduler.Components;
using LeagueScheduler.Features.Scheduling;
using LeagueScheduler.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var supportedCultures = builder.Configuration
    .GetSection("Localization:SupportedCultures")
    .Get<string[]>() ?? ["en-US"];
var defaultCulture = builder.Configuration["Localization:DefaultCulture"] ?? "en-US";

builder.Services.AddLocalization();
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<ScheduleOptions>(builder.Configuration.GetSection("Scheduling"));
builder.Services.AddScoped<IScheduleRepository, EfScheduleRepository>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();

var app = builder.Build();

// Apply any pending migrations on startup
using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

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

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(defaultCulture)
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LeagueScheduler.Client._Imports).Assembly);

app.MapSchedulingEndpoints();

app.Run();
