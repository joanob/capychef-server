using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Capychef.Persistence;

public class CapychefDbContextFactory : IDesignTimeDbContextFactory<CapychefDbContext>
{
    public CapychefDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CapychefDbContext>();

        Env.Load("../../.env");

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        optionsBuilder.UseNpgsql(connectionString);

        return new CapychefDbContext(optionsBuilder.Options);
    }
}