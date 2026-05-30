namespace Capychef.Api;

public static class CorsSetup
{
    public const string PolicyName = "Frontend";

    public static void SetupCors(IServiceCollection services)
    {
        var origins = (Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS") ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (origins.Length == 0)
            throw new InvalidOperationException("CORS_ALLOWED_ORIGINS environment variable not found or empty");

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
}

