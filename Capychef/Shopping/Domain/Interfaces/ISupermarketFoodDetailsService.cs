using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Shopping.Domain.Cmd;
using Capychef.Shopping.Domain.DTO;

namespace Capychef.Shopping.Domain.Interfaces;

public interface ISupermarketFoodDetailsService
{
    Task<Result<SupermarketFoodDetailsDto>> CreateSupermarketFoodDetails(
        AuthUserDetails userDetails, int foodId, SupermarketFoodDetailsCmd cmd);

    Task<AppError?> UpdateSupermarketFoodDetails(
        AuthUserDetails userDetails, int foodId, int id, SupermarketFoodDetailsCmd cmd);

    Task<AppError?> DeleteSupermarketFoodDetails(
        AuthUserDetails userDetails, int foodId, int id);
}