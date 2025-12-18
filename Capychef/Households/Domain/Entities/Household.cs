using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Utils;
using Capychef.Users.Domain.Entities;
using YourOwnBoss.Common.Entities;

namespace Capychef.Households.Domain.Entities;

[Table("households")]
public class Household : BaseDeletableEntity
{
    public Household()
    {
    }

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

    [Column("name")] public string Name { get; set; }

    [Column("public_id")] public string PublicId { get; set; }

    public User User { get; private set; } = null!;
}

public static class HouseholdExtensions
{
    public static IQueryable<Household> Active(this IQueryable<Household> households)
    {
        return households.Where(x => !x.IsDeleted);
    }
}