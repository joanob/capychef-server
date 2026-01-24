using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Food.Domain.Entities;
using Capychef.Households.Domain.Entities;

namespace Capychef.Storage.Domain.Entities;

[Table("batches")]
public class Batch : BaseDeletableEntity
{
    public Batch()
    {
    }

    public Batch(int householdId, int foodId, int storageSpaceId, double quantity, int foodUoMId)
    {
        HouseholdId = householdId;
        FoodId = foodId;
        StorageSpaceId = storageSpaceId;
        Quantity = quantity;
        FoodUoMId = foodUoMId;
        StoredAt = DateTime.UtcNow;
        IsConsumed = false;
        IsDiscarded = false;
    }


    [Column("household_id")] public int HouseholdId { get; set; }

    [Column("food_id")] public int FoodId { get; set; }

    [Column("storage_space_id")] public int StorageSpaceId { get; set; }

    [Column("quantity")] public double Quantity { get; set; }

    [Column("food_uom_id")] public int FoodUoMId { get; set; }

    [Column("stored_at")] public DateTime StoredAt { get; set; }

    [Column("original_batch_id")] public int? OriginalBatchId { get; set; }

    [Column("is_consumed")] public bool IsConsumed { get; set; }

    [Column("consumed_at")] public DateTime? ConsumedAt { get; set; }

    [Column("is_discarded")] public bool IsDiscarded { get; set; }

    [Column("discarded_at")] public DateTime? DiscardedAt { get; set; }

    [ForeignKey(nameof(HouseholdId))] public Household Household { get; set; }

    [ForeignKey(nameof(FoodId))] public Food.Domain.Entities.Food Food { get; set; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace StorageSpace { get; set; }

    [ForeignKey(nameof(FoodUoMId))] public FoodUoM FoodUoM { get; set; }

    [ForeignKey(nameof(OriginalBatchId))] public Batch? OriginalBatch { get; set; }
}