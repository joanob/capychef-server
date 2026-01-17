using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;
using YourOwnBoss.Common.Entities;

namespace Capychef.Households.Domain.Entities;

[Table("storage_spaces")]
public class StorageSpace : BaseDeletableEntity
{
    public StorageSpace()
    {
    }

    public StorageSpace(string name, StorageConditions storageCondition, int householdId, int userId)
    {
        Name = name;
        StorageCondition = storageCondition;
        HouseholdId = householdId;
        CreatedBy = userId;
    }

    public StorageSpace(string name, StorageConditions storageCondition, Household household, int userId)
    {
        Name = name;
        StorageCondition = storageCondition;
        Household = household;
        CreatedBy = userId;
    }

    [Column("name")] public string Name { get; set; }

    [Column("storage_condition")] public StorageConditions StorageCondition { get; set; }

    [Column("household_id")] public int HouseholdId { get; private set; }

    [Column("created_by")] public int CreatedBy { get; private set; }

    [ForeignKey(nameof(HouseholdId))] public Household Household { get; private set; }

    [ForeignKey(nameof(CreatedBy))] public User User { get; private set; }
}

public static class StorageSpaceExtensions
{
    public static IQueryable<StorageSpace> Active(this IQueryable<StorageSpace> storageSpaces)
    {
        return storageSpaces.Where(x => !x.IsDeleted);
    }
}