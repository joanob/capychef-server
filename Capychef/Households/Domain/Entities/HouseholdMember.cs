using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;
using YourOwnBoss.Common.Entities;

namespace Capychef.Households.Domain.Entities;

[Table("household_members")]
public class HouseholdMember : BaseDeletableEntity
{
    public HouseholdMember()
    {
    }

    public HouseholdMember(Household household, User user)
    {
        Household = household;
        User = user;
    }

    public HouseholdMember(Household household, int userId)
    {
        Household = household;
        UserId = userId;
    }

    public HouseholdMember(int householdId, int userId)
    {
        HouseholdId = householdId;
        UserId = userId;
    }

    [Column("household_id")]
    [ForeignKey(nameof(Household))]
    public int HouseholdId { get; private set; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; private set; }

    public Household Household { get; private set; } = null!;

    public User User { get; private set; } = null!;
}

public static class HouseholdMemberExtensions
{
    public static IQueryable<HouseholdMember> Active(this IQueryable<HouseholdMember> members)
    {
        return members.Where(x => !x.IsDeleted);
    }
}