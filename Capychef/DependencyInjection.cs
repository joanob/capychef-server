using Capychef.Persistence;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Capychef;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        Env.Load("../.env");

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        services.AddDbContext<CapychefDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<HealthCheckService, HealthCheckService>();

        return services;
    }
}