using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;

namespace Capychef.Households.Domain.Entities;

[Table("household_invitations")]
public class HouseholdInvitation
{
    protected HouseholdInvitation()
    {
    }

    public HouseholdInvitation(Household household, User user, int createdBy)
    {
        Household = household;
        User = user;
        CreatedBy = createdBy;
        IsAnswered = false;
        IsAccepted = false;
    }

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("user_id")] public int UserId { get; init; }

    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("is_answered")] public bool IsAnswered { get; private set; }

    [Column("answered_at")] public DateTime? AnsweredAt { get; private set; }

    [Column("is_accepted")] public bool IsAccepted { get; private set; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("row_version")]
    [ConcurrencyCheck]
    public int RowVersion { get; set; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; private set; }

    [ForeignKey(nameof(UserId))] public User? User { get; private set; }

    [ForeignKey(nameof(CreatedBy))] public User? Creator { get; private set; }

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

public static class HouseholdInvitationExtensions
{
    public static IQueryable<HouseholdInvitation> Active(this IQueryable<HouseholdInvitation> invitations)
    {
        return invitations.Where(x => !x.IsDeleted);
    }
}