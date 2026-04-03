using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Common.Utils;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Domain.Entities;

[Table("households")]
public class Household
{
    protected Household() {}
    
    public Household(User user, string name)
    {
        User = user;
        Name = name;
        PublicId = RandomGenerator.GenerateRandomCapsString(6);
        CreatedBy = user.Id;
    }

    public Household(int ownerId, string name)
    {
        OwnerId = ownerId;
        Name = name;
        PublicId = RandomGenerator.GenerateRandomCapsString(6);
        CreatedBy = ownerId;
    }
    
    [Column("id")] public int Id { get; init; }

    [Column("owner_id")]
    public int OwnerId { get; set; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("public_id")] [MaxLength(20)] public string PublicId { get; init; }
    
    [Column("created_at")] public DateTime CreatedAt { get; init; } =  DateTime.UtcNow;
    
    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; init; }
    
    [Column("is_deleted")] public bool IsDeleted { get; private set; }
    
    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [InverseProperty(nameof(StorageSpace.Household))]
    public ICollection<StorageSpace> StorageSpaces { get; } = new List<StorageSpace>();

    [ForeignKey(nameof(OwnerId))] public User? User { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
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