using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Domain.DTO;

public class HouseholdInvitationDto
{
    public HouseholdInvitationDto(HouseholdInvitation invitation)
    {
        Id = invitation.Id;
        HouseholdId = invitation.HouseholdId;
        User = new UserDto(invitation.User);
    }

    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public UserDto User { get; set; }

    public static List<HouseholdInvitationDto> ToList(List<HouseholdInvitation> invitations)
    {
        return invitations.Select(invitation => new HouseholdInvitationDto(invitation)).ToList();
    }
}