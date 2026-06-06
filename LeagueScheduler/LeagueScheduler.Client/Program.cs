using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using LeagueScheduler.Client.Features.Scheduling;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SchedulerClient>();

await builder.Build().RunAsync();
