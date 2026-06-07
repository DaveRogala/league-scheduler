using MudBlazor.Services;
using LeagueScheduler.Client.Features.Scheduling;
using LeagueScheduler.Components;
using LeagueScheduler.Features.Auth;
using LeagueScheduler.Features.Admin.TimeZones;
using LeagueScheduler.Features.Users;
using LeagueScheduler.Features.Courts;
using LeagueScheduler.Features.Leagues;
using LeagueScheduler.Features.Auth.Entities;
using LeagueScheduler.Features.Scheduling;
using LeagueScheduler.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is not configured. Add it to appsettings.Development.json or set the Jwt__Secret environment variable.");

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

builder.Services.AddIdentityCore<AppUser>(opts =>
{
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 8;
    opts.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<AppDbContext>()
.AddSignInManager();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();

builder.Services.Configure<ScheduleOptions>(builder.Configuration.GetSection("Scheduling"));
builder.Services.AddScoped<IScheduleRepository, EfScheduleRepository>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LeagueScheduler.Client._Imports).Assembly);

app.MapSchedulingEndpoints();
app.MapAuthEndpoints();
app.MapUserProfileEndpoints();
app.MapTimeZoneEndpoints();
app.MapLeagueEndpoints();
app.MapCourtEndpoints();

app.Run();
