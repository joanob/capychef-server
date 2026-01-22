using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdJoinRequestService
{
    Task<AppError?> CreateJoinRequest(AuthUserDetails userDetails, CreateHouseholdJoinRequestCmd cmd);
    Task<List<HouseholdJoinRequestDTO>> GetAllHouseholdJoinRequests(AuthUserDetails userDetails);
    Task<List<HouseholdJoinRequestDTO>> GetHouseholdJoinRequestsByUser(AuthUserDetails userDetails);
    Task<AppError?> AcceptJoinRequest(AuthUserDetails userDetails, int invitationId);
    Task<AppError?> RejectJoinRequest(AuthUserDetails userDetails, int invitationId);
}