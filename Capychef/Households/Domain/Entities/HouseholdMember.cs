using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Users.Domain.Entities;

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
        DidLeave = false;
    }

    public HouseholdMember(Household household, int userId)
    {
        Household = household;
        UserId = userId;
        DidLeave = false;
    }

    public HouseholdMember(int householdId, int userId)
    {
        HouseholdId = householdId;
        UserId = userId;
        DidLeave = false;
    }

    [Column("household_id")]
    [ForeignKey(nameof(Household))]
    public int HouseholdId { get; private set; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; private set; }

    [Column("did_leave")] public bool DidLeave { get; private set; }

    public Household Household { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public void Leave()
    {
        DidLeave = true;
        Delete();
    }
}

public static class HouseholdMemberExtensions
{
    public static IQueryable<HouseholdMember> Active(this IQueryable<HouseholdMember> members)
    {
        return members.Where(x => !x.IsDeleted);
    }
}