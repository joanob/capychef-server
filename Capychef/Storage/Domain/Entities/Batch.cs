using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Food.Domain.Entities;
using Capychef.Households.Domain.Entities;

namespace Capychef.Storage.Domain.Entities;

[Table("batches")]
public class Batch : BaseDeletableEntity
{
    public Batch(int userId, int householdId, int foodId, int storageSpaceId, double quantity, int foodUoMId,
        DateTime? bestBeforeDate, DateTime? expirationDate, bool isOpen, bool isConsumed, bool isDiscarded)
    {
        CreatedBy = userId;
        HouseholdId = householdId;
        FoodId = foodId;
        StorageSpaceId = storageSpaceId;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        BestBeforeDate = bestBeforeDate;
        ExpirationDate = expirationDate;
        IsOpen = isOpen;
        IsConsumed = isConsumed;
        IsDiscarded = isDiscarded;
    }

    public Batch(int userId, int householdId, int foodId, int storageSpaceId, double quantity, int foodUoMId,
        DateTime? bestBeforeDate, DateTime? expirationDate, int originalBatchId, bool isOpen, bool isConsumed,
        bool isDiscarded)
    {
        CreatedBy = userId;
        HouseholdId = householdId;
        FoodId = foodId;
        StorageSpaceId = storageSpaceId;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        BestBeforeDate = bestBeforeDate;
        ExpirationDate = expirationDate;
        OriginalBatchId = originalBatchId;
        IsOpen = isOpen;
        IsConsumed = isConsumed;
        IsDiscarded = isDiscarded;
    }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("food_id")] public int FoodId { get; init; }

    [Column("storage_space_id")] public int StorageSpaceId { get; set; }

    [Column("quantity")] public double Quantity { get; set; }

    [Column("food_uom_id")] public int FoodUoMId { get; set; }

    [Column("best_before_date")] public DateTime? BestBeforeDate { get; set; }

    [Column("expiration_date")] public DateTime? ExpirationDate { get; set; }

    [Column("original_batch_id")] public int? OriginalBatchId { get; init; }

    [Column("is_open")] public bool IsOpen { get; set; }

    [Column("is_consumed")] public bool IsConsumed { get; set; }

    [Column("consumed_at")] public DateTime? ConsumedAt { get; set; }

    [Column("is_discarded")] public bool IsDiscarded { get; set; }

    [Column("discarded_at")] public DateTime? DiscardedAt { get; set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; set; }

    [ForeignKey(nameof(FoodId))] public Food.Domain.Entities.Food? Food { get; set; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace? StorageSpace { get; set; }

    [ForeignKey(nameof(FoodUoMId))] public FoodUoM? FoodUoM { get; set; }

    [ForeignKey(nameof(OriginalBatchId))] public Batch? OriginalBatch { get; set; }
}