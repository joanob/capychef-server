using Capychef.Storage.Domain.Entities;

namespace Capychef.Storage.Domain.DTO;

public class BatchDto
{
    public BatchDto(Batch batch)
    {
        Id = batch.Id;
        HouseholdId = batch.HouseholdId;
        FoodId = batch.FoodId;
        StorageSpaceId = batch.StorageSpaceId;
        Quantity = batch.Quantity;
        FoodUoMId = batch.FoodUoMId;
        BestBeforeDate = batch.BestBeforeDate;
        ExpirationDate = batch.ExpirationDate;
        OriginalBatchId = batch.OriginalBatchId;
        IsOpen = batch.IsOpen;
        IsConsumed = batch.IsConsumed;
        ConsumedAt = batch.ConsumedAt;
        IsDiscarded = batch.IsDiscarded;
        DiscardedAt = batch.DiscardedAt;
    }


    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public int FoodId { get; set; }
    public int StorageSpaceId { get; set; }
    public double Quantity { get; set; }
    public int FoodUoMId { get; set; }
    public DateTime? BestBeforeDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int? OriginalBatchId { get; set; }
    public bool IsOpen { get; set; }
    public bool IsConsumed { get; set; }
    public DateTime? ConsumedAt { get; set; }
    public bool IsDiscarded { get; set; }
    public DateTime? DiscardedAt { get; set; }

    public static List<BatchDto> ToBatchDtoList(IEnumerable<Batch> batches)
    {
        return batches.Select(batch => new BatchDto(batch)).ToList();
    }
}