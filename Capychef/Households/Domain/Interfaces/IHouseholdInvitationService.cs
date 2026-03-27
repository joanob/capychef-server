using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdInvitationService
{
    Task<AppError?> CreateInvitation(AuthUserDetails userDetails, CreateHouseholdInvitationCmd cmd);
    Task<List<HouseholdInvitationDto>> GetAllHouseholdInvitations(AuthUserDetails userDetails);
    Task<List<HouseholdInvitationDto>> GetHouseholdInvitationsByUser(AuthUserDetails userDetails);
    Task<AppError?> AcceptInvitation(AuthUserDetails userDetails, int invitationId);
    Task<AppError?> RejectInvitation(AuthUserDetails userDetails, int invitationId);
}