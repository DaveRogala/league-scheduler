using MudBlazor.Services;
using LeagueScheduler.Client.Features.Scheduling;
using LeagueScheduler.Components;
using LeagueScheduler.Features.Admin.Logs;
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
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.Globalization;
using System.Text;

// Bootstrap logger captures startup errors before full Serilog config is ready.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection is not configured.");
    var jwtSecret = builder.Configuration["Jwt:Secret"]
        ?? throw new InvalidOperationException("Jwt:Secret is not configured. Add it to appsettings.Development.json or set the Jwt__Secret environment variable.");

    // Column map must match the LogEntryConfiguration EF schema.
    var logColumns = new Dictionary<string, ColumnWriterBase>
    {
        ["timestamp"]  = new TimestampColumnWriter(NpgsqlDbType.TimestampTz),
        ["level"]      = new LevelColumnWriter(renderAsText: true, dbType: NpgsqlDbType.Text),
        ["template"]   = new MessageTemplateColumnWriter(dbType: NpgsqlDbType.Text),
        ["message"]    = new RenderedMessageColumnWriter(dbType: NpgsqlDbType.Text),
        ["exception"]  = new ExceptionColumnWriter(dbType: NpgsqlDbType.Text),
        ["properties"] = new PropertiesColumnWriter(dbType: NpgsqlDbType.Jsonb),
    };

    builder.Host.UseSerilog((ctx, services, config) => config
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.PostgreSQL(
            connectionString: connectionString,
            tableName: "Logs",
            columnOptions: logColumns,
            needAutoCreateTable: false,
            schemaName: "public"));

    var supportedCultures = builder.Configuration
        .GetSection("Localization:SupportedCultures")
        .Get<string[]>() ?? ["en-US"];
    var defaultCulture = builder.Configuration["Localization:DefaultCulture"] ?? "en-US";

    builder.Services.AddLocalization();
    builder.Services.AddMudServices();
    builder.Services.AddRazorComponents()
        .AddInteractiveWebAssemblyComponents();

    builder.Services.AddDbContext<AppDbContext>(opts =>
        opts.UseNpgsql(connectionString));

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
    app.MapLogEndpoints();

    Log.Information("Application starting");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
