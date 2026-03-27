using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;

namespace Capychef.Households.Domain.Interfaces;

public interface IStorageSpaceService
{
    public Task<Result<StorageSpaceDto>> CreateStorageSpace(StorageSpaceCmd cmd, AuthUserDetails userDetails);
    Task<Result<StorageSpaceDto>> UpdateStorageSpace(int id, StorageSpaceCmd cmd, AuthUserDetails userDetails);
    Task<AppError?> DeleteStorageSpace(int id, AuthUserDetails userDetails);
    Task<Result<List<StorageSpaceDto>>> GetHouseholdStorageSpaces(AuthUserDetails userDetails);
    Task<Result<List<InitialStorageSpaceDto>>> GetInitialStorageSpaces();
}