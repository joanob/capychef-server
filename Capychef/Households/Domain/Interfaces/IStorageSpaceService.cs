using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Households.Domain.Interfaces;

public interface IStorageSpaceService
{
    public Task<Result<StorageSpaceDTO>> CreateStorageSpace(CreateStorageSpaceCmd cmd, AuthUserDetails userDetails);
    Task<Result<StorageSpaceDTO>> UpdateStorageSpace(int id, UpdateStorageSpaceCmd cmd, AuthUserDetails userDetails);
    Task<AppError> DeleteStorageSpace(int id, AuthUserDetails userDetails);
}