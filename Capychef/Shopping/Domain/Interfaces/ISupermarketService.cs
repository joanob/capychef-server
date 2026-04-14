using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Shopping.Domain.Cmd;
using Capychef.Shopping.Domain.DTO;

namespace Capychef.Shopping.Domain.Interfaces;

public interface ISupermarketService
{
    Task<Result<SupermarketDto>> CreateSupermarket(AuthUserDetails userDetails, SupermarketCmd cmd);
    Task<AppError?> UpdateSupermarket(AuthUserDetails userDetails, int id, SupermarketCmd cmd);
    Task<AppError?> DeleteSupermarket(AuthUserDetails userDetails, int id);
}