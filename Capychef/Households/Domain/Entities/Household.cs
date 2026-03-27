using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Common.Utils;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Domain.Entities;

[Table("households")]
public class Household : BaseDeletableEntity
{
    public Household(User user, string name)
    {
        User = user;
        Name = name;
        PublicId = RandomGenerator.GenerateRandomCapsString(6);
    }

    public Household(int ownerId, string name)
    {
        OwnerId = ownerId;
        Name = name;
        PublicId = RandomGenerator.GenerateRandomCapsString(6);
    }

    [Column("owner_id")]
    [ForeignKey(nameof(User))]
    public int OwnerId { get; private set; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("public_id")] [MaxLength(50)] public string PublicId { get; set; }

    [InverseProperty(nameof(StorageSpace.Household))]
    public ICollection<StorageSpace> StorageSpaces { get; } = new List<StorageSpace>();

    public User User { get; private set; } = null!;

    public void AddStorageSpace(StorageSpace storageSpace)
    {
        StorageSpaces.Add(storageSpace);
    }
}

public static class HouseholdExtensions
{
    public static IQueryable<Household> Active(this IQueryable<Household> households)
    {
        return households.Where(x => !x.IsDeleted);
    }

    public static IQueryable<Household> IncludeStorageSpaces(this IQueryable<Household> household)
    {
        return household.Include(x => x.StorageSpaces);
    }
}