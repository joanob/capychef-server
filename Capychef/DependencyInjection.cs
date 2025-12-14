using Capychef.Infrastructure.DevImplementations;
using Capychef.Infrastructure.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;
using Capychef.Users.Repositories;
using Capychef.Users.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Capychef;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        // DATABASE

        Env.Load("../.env");

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        services.AddDbContext<CapychefDbContext>(options =>
            options.UseNpgsql(connectionString));

        // EMAIL

        services.AddScoped<IEmailSender, Smtp4DevSender>();

        // HEALTH

        services.AddScoped<HealthCheckService, HealthCheckService>();

        // USERS

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();
        services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}