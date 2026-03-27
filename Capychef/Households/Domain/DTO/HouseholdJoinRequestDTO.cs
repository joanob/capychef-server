using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.DTO;

public class HouseholdJoinRequestDto
{
    public HouseholdJoinRequestDto(HouseholdJoinRequest invitation)
    {
        Id = invitation.Id;
        HouseholdId = invitation.HouseholdId;
        UserId = invitation.UserId;
    }

    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public int UserId { get; set; }

    public static List<HouseholdJoinRequestDto> ToList(List<HouseholdJoinRequest> invitations)
    {
        return invitations.Select(invitation => new HouseholdJoinRequestDto(invitation)).ToList();
    }
}