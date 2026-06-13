using System.Text.Json;
using Capychef;
using Capychef.Api;
using Capychef.Api.Auth;
using Capychef.Api.Errors;
using Capychef.Api.Logging;
using Capychef.Api.Realtime;
using DotNetEnv;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

var environment = GetEnvironment(args, builder.Configuration);
builder.Environment.EnvironmentName = environment;
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

if (environment == AppEnvironment.Dev)
{
    var envFile = $"../.env.{environment.ToLower()}";
    Env.Load(envFile);
}

// Add services to the container.

SerilogSetup.SetupSerilog(builder.Environment);

builder.Host.UseSerilog();

builder.Services.AddRealtimeDi();
builder.Services.AddApplicationDi();

RateLimiterSetup.SetupRateLimiter(builder.Services, builder.Environment);

CacheSetup.SetupCache(builder.Services, builder.Environment);

CorsSetup.SetupCors(builder.Services);

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDev())
{
    try
    {
        var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
        swaggerProvider.GetSwagger("v1");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error generating Swagger document at startup:");
        Console.WriteLine(ex.ToString());
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsSetup.PolicyName);

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRateLimiter();

app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.MapHub<RealtimeHub>("/realtime");

app.Run();

static string GetEnvironment(string[] args, IConfiguration configuration)
{
    var envVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                 ?? configuration["ASPNETCORE_ENVIRONMENT"];

    if (!string.IsNullOrEmpty(envVar) && IsValidEnvironment(envVar))
    {
        Console.WriteLine($"Environment from system variable: {envVar.ToUpper()}");
        return envVar.ToUpper();
    }

    var environmentArg = args
        .FirstOrDefault(arg => arg.StartsWith("--environment", StringComparison.OrdinalIgnoreCase)
                               || arg.StartsWith("-e", StringComparison.OrdinalIgnoreCase));

    if (!string.IsNullOrEmpty(environmentArg))
    {
        if (environmentArg.Contains('='))
        {
            var value = environmentArg.Split('=')[1].Trim().ToUpper();
            if (IsValidEnvironment(value))
            {
                Console.WriteLine($"Environment from CLI argument: {value}");
                return value;
            }
        }
        else
        {
            var index = Array.IndexOf(args, environmentArg);
            if (index < args.Length - 1)
            {
                var value = args[index + 1].Trim().ToUpper();
                if (IsValidEnvironment(value))
                {
                    Console.WriteLine($"Environment from CLI argument: {value}");
                    return value;
                }
            }
        }
    }

    Console.WriteLine(
        "No environment specified. Using DEV by default. Set ASPNETCORE_ENVIRONMENT or use --environment=<DEV|QA|PROD> to change.");
    return AppEnvironment.Dev;
}

static bool IsValidEnvironment(string env)
{
    return env.Equals(AppEnvironment.Dev, StringComparison.OrdinalIgnoreCase)
           || env.Equals(AppEnvironment.Qa, StringComparison.OrdinalIgnoreCase)
           || env.Equals(AppEnvironment.Prod, StringComparison.OrdinalIgnoreCase);
}