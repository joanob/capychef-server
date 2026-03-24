using Capychef.Food.Domain.Entities;
using Capychef.Gourmet.Domain.Entities;
using Capychef.Households.Domain.Entities;
using Capychef.Storage.Domain.Entities;
using Capychef.Users.Domain.Entities;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Persistence;

public class CapychefDbContext(DbContextOptions<CapychefDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserPassword> UsersPasswords => Set<UserPassword>();
    public DbSet<UserSession> UsersSessions => Set<UserSession>();
    public DbSet<UserToken> UsersTokens => Set<UserToken>();

    public DbSet<Household> Households => Set<Household>();
    public DbSet<HouseholdMember> HouseholdMembers => Set<HouseholdMember>();
    public DbSet<HouseholdInvitation> HouseholdInvitations => Set<HouseholdInvitation>();
    public DbSet<HouseholdJoinRequest> HouseholdJoinRequests => Set<HouseholdJoinRequest>();

    public DbSet<SubscriptionDiscount> SubscriptionDiscounts => Set<SubscriptionDiscount>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    public DbSet<InitialStorageSpace> InitialStorageSpaces => Set<InitialStorageSpace>();
    public DbSet<StorageSpace> StorageSpaces => Set<StorageSpace>();

    public DbSet<StorageSpacesModificationHistory> StorageSpacesModificationsHistory =>
        Set<StorageSpacesModificationHistory>();

    public DbSet<FoodCategory> FoodCategories => Set<FoodCategory>();
    public DbSet<UoMDimension> UoMDimensions => Set<UoMDimension>();
    public DbSet<UoM> UoM => Set<UoM>();
    public DbSet<Food.Domain.Entities.Food> Food => Set<Food.Domain.Entities.Food>();
    public DbSet<HouseholdFoodDetails> HouseholdFoodDetails => Set<HouseholdFoodDetails>();
    public DbSet<FoodUoM> FoodUoM => Set<FoodUoM>();
    public DbSet<FoodModificationHistory> FoodModificationsHistory => Set<FoodModificationHistory>();

    public DbSet<Batch> Batches => Set<Batch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        InitialStorageSpace.OnModelCreating(modelBuilder);
        StorageSpace.OnModelCreating(modelBuilder);

        SubscriptionDiscount.OnModelCreating(modelBuilder);
        Subscription.OnModelCreating(modelBuilder);

        StorageSpacesModificationHistory.OnModelCreating(modelBuilder);
        FoodModificationHistory.OnModelCreating(modelBuilder);

        // Save all dates as UTC
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        foreach (var property in entityType.GetProperties())
        {
            if (property.Name == "RowVersion")
            {
                property.IsConcurrencyToken = true;
                property.ValueGenerated = ValueGenerated.Never;
            }

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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Modified))
        {
            var prop = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "RowVersion");
            if (prop != null)
            {
                var current = (int?)(prop.OriginalValue ?? 0);
                entry.CurrentValues["RowVersion"] = (current ?? 0) + 1;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Env.Load("../../.env");

        if (Environment.GetEnvironmentVariable("ENVIRONMENT") == "Development")
            optionsBuilder.EnableSensitiveDataLogging();
    }
}