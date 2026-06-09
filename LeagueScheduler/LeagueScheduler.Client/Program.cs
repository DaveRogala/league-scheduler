using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using LeagueScheduler.Client.Features.Auth;
using LeagueScheduler.Client.Features.Courts;
using LeagueScheduler.Client.Features.Admin.TimeZones;
using LeagueScheduler.Client.Features.Users;
using LeagueScheduler.Client.Features.Players;
using LeagueScheduler.Client.Features.Leagues;
using LeagueScheduler.Client.Features.Admin.Logs;
using LeagueScheduler.Client.Features.Common;
using LeagueScheduler.Client.Features.MatchTypes;
using LeagueScheduler.Client.Features.Scheduling;
using System.Globalization;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AppAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AppAuthStateProvider>());
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddScoped(sp =>
{
    var auth = sp.GetRequiredService<AppAuthStateProvider>();
    var handler = new BearerTokenHandler(auth)
    {
        InnerHandler = new HttpClientHandler()
    };
    return new HttpClient(handler)
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };
});

builder.Services.AddScoped<SchedulerClient>();
builder.Services.AddScoped<AuthClient>();
builder.Services.AddScoped<LeagueClient>();
builder.Services.AddScoped<CourtClient>();
builder.Services.AddScoped<UserProfileClient>();
builder.Services.AddScoped<TimeZoneClient>();
builder.Services.AddScoped<AdminLogClient>();
builder.Services.AddScoped<CourtHistoryClient>();
builder.Services.AddScoped<PronounsClient>();
builder.Services.AddScoped<CountryClient>();
builder.Services.AddScoped<MatchTypeClient>();
builder.Services.AddScoped<PlayerProfileClient>();

var host = builder.Build();

var js = host.Services.GetRequiredService<IJSRuntime>();
var cultureName = await js.InvokeAsync<string?>("blazorCulture.get");
if (!string.IsNullOrWhiteSpace(cultureName))
{
    try
    {
        var culture = new CultureInfo(cultureName);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
    catch (CultureNotFoundException)
    {
        // Stored culture is invalid or not available — fall back to default.
    }
}

await host.RunAsync();
