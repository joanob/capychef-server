using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdService
{
    Task<Result<HouseholdDto>> CreateHousehold(AuthUserDetails userDetails, CreateHouseholdCmd cmd);
    Task<AppError?> CheckHouseholdOwnership(AuthUserDetails userDetails);
    Task<AppError?> CheckHouseholdMembership(AuthUserDetails userDetails);
    Task<List<HouseholdDto>> GetAllHouseholds(AuthUserDetails userDetails);
    Task<Result<HouseholdDto>> GetHouseholdById(AuthUserDetails userDetails, int householdId);
    Task<Result<HouseholdDto>> SelectHousehold(AuthUserDetails userDetails, int householdId);
    Task<Result<HouseholdDto>> UpdateHousehold(AuthUserDetails userDetails, int householdId, HouseholdCmd cmd);
    Task<Result<HouseholdDto>> GetActiveHousehold(AuthUserDetails userDetails);
    Task<AppError?> DeleteHousehold(AuthUserDetails userDetails, int householdId);
}