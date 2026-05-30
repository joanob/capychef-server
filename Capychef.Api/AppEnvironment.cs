namespace Capychef.Api;

public static class AppEnvironment
{
    public const string Dev = "DEV";
    public const string Qa = "QA";
    public const string Prod = "PROD";
}

public static class HostEnvironmentExtensions
{
    public static bool IsDev(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(AppEnvironment.Dev);
    }

    public static bool IsQa(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(AppEnvironment.Qa);
    }

    public static bool IsProd(this IWebHostEnvironment env)
    {
        return env.IsEnvironment(AppEnvironment.Prod);
    }
}