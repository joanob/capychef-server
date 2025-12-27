using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class HouseholdJoinRequestDTO
{
    public HouseholdJoinRequestDTO(HouseholdJoinRequest invitation)
    {
        Id = invitation.Id;
        HouseholdId = invitation.HouseholdId;
        UserId = invitation.UserId;
    }

    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public int UserId { get; set; }

    public static List<HouseholdJoinRequestDTO> toList(List<HouseholdJoinRequest> invitations)
    {
        return invitations.Select(invitation => new HouseholdJoinRequestDTO(invitation)).ToList();
    }
}