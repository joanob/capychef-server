using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using YourOwnBoss.Common.Errors;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdInvitationService
{
    Task<AppError?> CreateInvitation(AuthUserDetails userDetails, CreateHouseholdInvitationCmd cmd);
    Task<List<HouseholdInvitationDTO>> GetAllHouseholdInvitations(AuthUserDetails userDetails);
}