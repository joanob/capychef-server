using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Utils;
using Capychef.Users.Domain.Entities;
using YourOwnBoss.Common.Entities;

namespace Capychef.Households.Domain.Entities;

[Table("household_invitations")]
public class HouseholdInvitation : BaseDeletableEntity
{
    public HouseholdInvitation()
    {
    }

    public HouseholdInvitation(Household household, User user)
    {
        Household = household;
        User = user;
        IsAnswered = false;
        IsAccepted = false;
    }
    
    [Column("household_id")]
    [ForeignKey(nameof(Household))]
    public int HouseholdId { get; private set; }
    
    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; private set; }

    [Column("is_answered")] public bool IsAnswered { get; set; }

    [Column("answered_at")] public bool AnsweredAt { get; set; }

    [Column("is_accepted")] public bool IsAccepted { get; set; }

    public Household Household { get; private set; } = null!;
    
    public User User { get; private set; } = null!;
}

public static class HouseholdInvitationExtensions
{
    public static IQueryable<HouseholdInvitation> Active(this IQueryable<HouseholdInvitation> invitations)
    {
        return invitations.Where(x => !x.IsDeleted);
    }
    
    public static IQueryable<HouseholdInvitation> Pending(this IQueryable<HouseholdInvitation> invitations)
    {
        return invitations.Where(x => !x.IsDeleted && !x.IsAnswered);
    }
}