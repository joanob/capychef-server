using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;

namespace Capychef.Households.Domain.Entities;

[Table("household_join_requests")]
public class HouseholdJoinRequest
{
    protected HouseholdJoinRequest()
    {
    }

    public HouseholdJoinRequest(Household household, User user)
    {
        Household = household;
        User = user;
        IsAnswered = false;
        IsAccepted = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("user_id")] public int UserId { get; init; }

    [Column("is_answered")] public bool IsAnswered { get; private set; }

    [Column("answered_at")] public DateTime? AnsweredAt { get; private set; }

    [Column("is_accepted")] public bool IsAccepted { get; private set; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("row_version")] public int RowVersion { get; init; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; private set; }

    [ForeignKey(nameof(UserId))] public User? User { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Accept()
    {
        IsAnswered = true;
        AnsweredAt = DateTime.UtcNow;
        IsAccepted = true;
    }

    public void Reject()
    {
        IsAnswered = true;
        AnsweredAt = DateTime.UtcNow;
        IsAccepted = false;
    }
}

public static class HouseholdJoinRequestExtensions
{
    public static IQueryable<HouseholdJoinRequest> Active(this IQueryable<HouseholdJoinRequest> requests)
    {
        return requests.Where(x => !x.IsDeleted);
    }

    public static IQueryable<HouseholdJoinRequest> Pending(this IQueryable<HouseholdJoinRequest> requests)
    {
        return requests.Where(x => !x.IsDeleted && !x.IsAnswered);
    }
}