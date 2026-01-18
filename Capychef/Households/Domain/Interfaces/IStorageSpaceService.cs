using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using YourOwnBoss.Common.Result;

namespace Capychef.Households.Domain.Interfaces;

public interface IStorageSpaceService
{
    public Task<Result<StorageSpaceDTO>> CreateStorageSpace(CreateStorageSpaceCmd cmd, AuthUserDetails userDetails);
}