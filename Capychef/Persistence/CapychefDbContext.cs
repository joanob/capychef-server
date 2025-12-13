using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Persistence;

public class CapychefDbContext(DbContextOptions<CapychefDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Save all dates as UTC
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        foreach (var property in entityType.GetProperties())
        {
            if (property.ClrType == typeof(DateTime))
                property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                ));

            if (property.ClrType == typeof(DateTime?))
                property.SetValueConverter(new ValueConverter<DateTime?, DateTime?>(
                    v => v == null ? null : v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime(),
                    v => v == null ? null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                ));
        }
    }
}