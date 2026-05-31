using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Domain.Entities;

[Table("storage_spaces")]
public class StorageSpace
{
    protected StorageSpace()
    {
        Name = "";
        StorageCondition = StorageConditions.From("");
    }

    public StorageSpace(string name, StorageConditions storageCondition, int householdId, int userId)
    {
        HouseholdId = householdId;
        Name = name;
        StorageCondition = storageCondition;
        CreatedBy = userId;
    }

    public StorageSpace(string name, StorageConditions storageCondition, Household household, int userId)
    {
        Name = name;
        StorageCondition = storageCondition;
        Household = household;
        CreatedBy = userId;
    }

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("storage_condition")] public StorageConditions StorageCondition { get; set; }

    [Column("created_at")] public DateTime? CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")]
    [ConcurrencyCheck]
    public int RowVersion { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; init; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageSpace>()
            .Property(f => f.StorageCondition)
            .HasConversion(new StorageConditionsConverter());
    }
}

public static class StorageSpaceExtensions
{
    public static IQueryable<StorageSpace> Active(this IQueryable<StorageSpace> storageSpaces)
    {
        return storageSpaces.Where(x => !x.IsDeleted);
    }
}