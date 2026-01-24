using Capychef.Common.Auth;
using Capychef.Common.Result;
using Capychef.Storage.Domain.Cmd;
using Capychef.Storage.Domain.DTO;

namespace Capychef.Storage.Domain.Interfaces;

public interface IBatchService
{
    Task<Result<BatchDTO>> CreateBatch(AuthUserDetails userDetails, CreateBatchCmd cmd);
    Task<List<BatchDTO>> GetAllBatches(AuthUserDetails userDetails);
}