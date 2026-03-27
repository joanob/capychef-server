using Capychef.Persistence;

namespace Capychef;

public class HealthCheckService(CapychefDbContext dbContext)
{
    public async Task<string> CheckServiceHealth()
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        if (!canConnect) return "Cannot connect to database";

        return "All services are running";
    }
}