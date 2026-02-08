using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Domain.DTO;

public class HouseholdInvitationDTO
{
    public HouseholdInvitationDTO(HouseholdInvitation invitation)
    {
        Id = invitation.Id;
        HouseholdId = invitation.HouseholdId;
        User = new UserDTO(invitation.User);
    }

    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public UserDTO User { get; set; }

    public static List<HouseholdInvitationDTO> toList(List<HouseholdInvitation> invitations)
    {
        return invitations.Select(invitation => new HouseholdInvitationDTO(invitation)).ToList();
    }
}