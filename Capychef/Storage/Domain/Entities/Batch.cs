using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Food.Domain.Entities;
using Capychef.Households.Domain.Entities;

namespace Capychef.Storage.Domain.Entities;

[Table("batches")]
public class Batch
{
    protected Batch()
    {
    }

    [Column("id")] public int Id { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("food_id")] public int FoodId { get; init; }

    [Column("storage_space_id")] public int StorageSpaceId { get; set; }

    [Column("quantity")] public double Quantity { get; set; }

    [Column("food_uom_id")] public int FoodUoMId { get; set; }

    [Column("stored_at")] public DateTime StoredAt { get; set; }

    [Column("best_before_date")] public DateTime? BestBeforeDate { get; set; }

    [Column("expiration_date")] public DateTime? ExpirationDate { get; set; }

    [Column("original_batch_id")] public int? OriginalBatchId { get; init; }

    [Column("is_consumed")] public bool IsConsumed { get; private set; }

    [Column("consumed_at")] public DateTime? ConsumedAt { get; private set; }

    [Column("is_discarded")] public bool IsDiscarded { get; private set; }

    [Column("discarded_at")] public DateTime? DiscardedAt { get; private set; }

    [Column("created_at")] public DateTime CreatedAt { get; init; }

    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; init; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; init; }

    [ForeignKey(nameof(FoodId))] public Food.Domain.Entities.Food? Food { get; init; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace? StorageSpace { get; init; }

    [ForeignKey(nameof(FoodUoMId))] public FoodUoM? FoodUoM { get; init; }

    [ForeignKey(nameof(OriginalBatchId))] public Batch? OriginalBatch { get; init; }

    public static Batch New(int householdId, int foodId, int storageSpaceId, double quantity, int foodUoMId,
        DateTime? bestBeforeDate, DateTime? expirationDate)
    {
        return new Batch
        {
            HouseholdId = householdId,
            FoodId = foodId,
            StorageSpaceId = storageSpaceId,
            Quantity = quantity,
            FoodUoMId = foodUoMId,
            StoredAt = DateTime.UtcNow,
            BestBeforeDate = bestBeforeDate,
            ExpirationDate = expirationDate,
            IsConsumed = false,
            IsDiscarded = false
        };
    }

    public static Batch FromOriginal(Batch originalBatch, double quantity)
    {
        return new Batch
        {
            HouseholdId = originalBatch.HouseholdId,
            FoodId = originalBatch.FoodId,
            StorageSpaceId = originalBatch.StorageSpaceId,
            Quantity = quantity,
            FoodUoMId = originalBatch.FoodUoMId,
            StoredAt = DateTime.UtcNow,
            BestBeforeDate = originalBatch.BestBeforeDate,
            ExpirationDate = originalBatch.ExpirationDate,
            OriginalBatch = originalBatch,
            IsConsumed = false,
            IsDiscarded = false
        };
    }

    public void Consume()
    {
        IsConsumed = true;
        ConsumedAt = DateTime.UtcNow;
    }

    public void Discard()
    {
        IsDiscarded = true;
        DiscardedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}