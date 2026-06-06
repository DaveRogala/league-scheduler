using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using LeagueScheduler.Client.Features.Scheduling;
using System.Globalization;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SchedulerClient>();

var host = builder.Build();

var js = host.Services.GetRequiredService<IJSRuntime>();
var cultureName = await js.InvokeAsync<string?>("blazorCulture.get");
if (!string.IsNullOrWhiteSpace(cultureName))
{
    var culture = new CultureInfo(cultureName);
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

await host.RunAsync();
