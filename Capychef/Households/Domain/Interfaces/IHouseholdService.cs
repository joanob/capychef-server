using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdService
{
    Task<Result<HouseholdDTO>> CreateHousehold(AuthUserDetails userDetails, CreateHouseholdCmd cmd);
    Task<AppError?> CheckHouseholdOwnership(AuthUserDetails userDetails);
    Task<AppError?> CheckHouseholdMembership(AuthUserDetails userDetails);
    Task<List<HouseholdDTO>> GetAllHouseholds(AuthUserDetails userDetails);
    Task<Result<HouseholdDTO>> SelectHousehold(AuthUserDetails userDetails, int householdId);
}