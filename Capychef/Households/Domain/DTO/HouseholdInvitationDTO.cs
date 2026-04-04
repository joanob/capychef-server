using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Domain.DTO;

public class HouseholdInvitationDto
{
    public HouseholdInvitationDto(HouseholdInvitation invitation)
    {
        Id = invitation.Id;
        HouseholdId = invitation.HouseholdId;
        if (invitation.User == null) throw new Exception("User can't be null in HouseholdInvitationDto.");
        User = new UserDto(invitation.User);
    }

    public int Id { get; }
    public int HouseholdId { get; }
    public UserDto User { get; }

    public static List<HouseholdInvitationDto> ToList(List<HouseholdInvitation> invitations)
    {
        return invitations.Select(invitation => new HouseholdInvitationDto(invitation)).ToList();
    }
}