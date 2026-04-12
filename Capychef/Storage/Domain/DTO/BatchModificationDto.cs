using Capychef.Storage.Domain.Entities;

namespace Capychef.Storage.Domain.DTO;

/// <summary>
///     Result of a batch modification operation (Move, Consume, Discard).
///     Contains the original batch (modified) and optionally a new batch if it was created.
/// </summary>
public class BatchModificationDto
{
    public BatchModificationDto(Batch originalBatch, Batch? newBatch = null)
    {
        OriginalBatch = new BatchDto(originalBatch);
        if (newBatch != null) NewBatch = new BatchDto(newBatch);
    }

    public BatchDto OriginalBatch { get; init; }
    public BatchDto? NewBatch { get; init; }
}