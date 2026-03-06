using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdService
{
    Task<Result<HouseholdDTO>> CreateHousehold(AuthUserDetails userDetails, HouseholdCmd cmd);
    Task<AppError?> CheckHouseholdOwnership(AuthUserDetails userDetails);
    Task<AppError?> CheckHouseholdMembership(AuthUserDetails userDetails);
    Task<List<HouseholdDTO>> GetAllHouseholds(AuthUserDetails userDetails);
    Task<Result<HouseholdDTO>> GetHouseholdById(AuthUserDetails userDetails, int householdId);
    Task<Result<HouseholdDTO>> SelectHousehold(AuthUserDetails userDetails, int householdId);
    Task<Result<HouseholdDTO>> UpdateHousehold(AuthUserDetails userDetails, int householdId, HouseholdCmd cmd);
    Task<Result<HouseholdDTO>> GetActiveHousehold(AuthUserDetails userDetails);
}