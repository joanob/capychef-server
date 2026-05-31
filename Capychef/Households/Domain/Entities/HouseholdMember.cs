using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;

namespace Capychef.Households.Domain.Entities;

[Table("household_members")]
public class HouseholdMember
{
    protected HouseholdMember()
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

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("user_id")] public int UserId { get; init; }

    [Column("did_leave")] public bool DidLeave { get; private set; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("row_version")]
    [ConcurrencyCheck]
    public int RowVersion { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; private set; }

    [ForeignKey(nameof(UserId))] public User? User { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

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