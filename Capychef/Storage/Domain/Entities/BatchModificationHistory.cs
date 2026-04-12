using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Food.Domain.Entities;
using Capychef.Households.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Storage.Domain.Entities;

[Table("batch_modifications_history")]
public class BatchModificationHistory
{
    protected BatchModificationHistory()
    {
        ModificationType = BatchModificationType.From("");
    }

    [Column("id")] public int Id { get; init; }

    [Column("batch_id")] public int BatchId { get; init; }

    [Column("modification_type")] public BatchModificationType ModificationType { get; init; }

    [Column("storage_space_id")] public int StorageSpaceId { get; init; }

    [Column("previous_quantity")] public double? PreviousQuantity { get; init; }

    [Column("previous_food_uom_id")] public int? PreviousFoodUoMId { get; init; }
    [Column("delta_quantity")] public double? DeltaQuantity { get; init; }

    [Column("delta_food_uom_id")] public int? DeltaFoodUoMId { get; init; }

    [Column("new_quantity")] public double NewQuantity { get; init; }

    [Column("new_food_uom_id")] public int NewFoodUoMId { get; init; }

    [Column("best_before_date")] public DateTime? BestBeforeDate { get; init; }

    [Column("expiration_date")] public DateTime? ExpirationDate { get; init; }

    [Column("created_from_batch_id")] public int? CreatedFromBatchId { get; init; }

    [Column("created_new_batch_id")] public int? CreatedNewBatchId { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; init; }

    [Column("created_by")] public int CreatedBy { get; init; }

    [ForeignKey(nameof(BatchId))] public Batch? Batch { get; init; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace? StorageSpace { get; init; }

    [ForeignKey(nameof(PreviousFoodUoMId))]
    public FoodUoM? PreviousFoodUoM { get; init; }

    [ForeignKey(nameof(DeltaFoodUoMId))] public FoodUoM? DeltaFoodUoM { get; init; }

    [ForeignKey(nameof(NewFoodUoMId))] public FoodUoM? NewFoodUoM { get; init; }

    [ForeignKey(nameof(CreatedFromBatchId))]
    public Batch? CreatedFromBatch { get; init; }

    [ForeignKey(nameof(CreatedNewBatchId))]
    public Batch? CreatedNewBatch { get; init; }

    public static BatchModificationHistory Initial(Batch batch, int userId)
    {
        return new BatchModificationHistory
        {
            Batch = batch,
            ModificationType = BatchModificationType.Initial,
            StorageSpaceId = batch.StorageSpaceId,
            NewQuantity = batch.Quantity,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory FullMove(Batch batch, int userId, int newStorageSpaceId)
    {
        return new BatchModificationHistory
        {
            BatchId = batch.Id,
            ModificationType = BatchModificationType.FullMove,
            StorageSpaceId = newStorageSpaceId,
            NewQuantity = batch.Quantity,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory OriginalBatchPartialMove(Batch batch, Batch newBatch, int userId,
        double previousQuantity, int previousFoodUoMId)
    {
        return new BatchModificationHistory
        {
            BatchId = batch.Id,
            ModificationType = BatchModificationType.PartialMove,
            StorageSpaceId = batch.StorageSpaceId,
            PreviousQuantity = previousQuantity,
            DeltaQuantity = newBatch.Quantity,
            NewQuantity = batch.Quantity,
            PreviousFoodUoMId = previousFoodUoMId,
            DeltaFoodUoMId = newBatch.FoodUoMId,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate,
            CreatedNewBatch = newBatch,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory NewBatchPartialMove(Batch newBatch, Batch originalBatch, int userId)
    {
        return new BatchModificationHistory
        {
            Batch = newBatch,
            ModificationType = BatchModificationType.PartialMove,
            StorageSpaceId = newBatch.StorageSpaceId,
            NewQuantity = newBatch.Quantity,
            NewFoodUoMId = newBatch.FoodUoMId,
            BestBeforeDate = newBatch.BestBeforeDate,
            ExpirationDate = newBatch.ExpirationDate,
            CreatedFromBatch = originalBatch,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory FullConsume(Batch batch, int userId)
    {
        return new BatchModificationHistory
        {
            BatchId = batch.Id,
            ModificationType = BatchModificationType.FullConsume,
            StorageSpaceId = batch.StorageSpaceId,
            NewQuantity = batch.Quantity,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory OriginalBatchPartialConsume(Batch batch, Batch consumedBatch, int userId,
        double previousQuantity, int previousFoodUoMId)
    {
        return new BatchModificationHistory
        {
            BatchId = batch.Id,
            ModificationType = BatchModificationType.PartialConsume,
            StorageSpaceId = batch.StorageSpaceId,
            PreviousQuantity = previousQuantity,
            DeltaQuantity = consumedBatch.Quantity,
            NewQuantity = batch.Quantity,
            PreviousFoodUoMId = previousFoodUoMId,
            DeltaFoodUoMId = consumedBatch.FoodUoMId,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate,
            CreatedNewBatch = consumedBatch,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory ConsumedBatchPartialConsume(Batch consumedBatch, Batch originalBatch,
        int userId)
    {
        return new BatchModificationHistory
        {
            Batch = consumedBatch,
            ModificationType = BatchModificationType.PartialConsume,
            StorageSpaceId = consumedBatch.StorageSpaceId,
            NewQuantity = consumedBatch.Quantity,
            NewFoodUoMId = consumedBatch.FoodUoMId,
            BestBeforeDate = consumedBatch.BestBeforeDate,
            ExpirationDate = consumedBatch.ExpirationDate,
            CreatedFromBatch = originalBatch,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory FullDiscard(Batch batch, int userId)
    {
        return new BatchModificationHistory
        {
            BatchId = batch.Id,
            ModificationType = BatchModificationType.FullDiscard,
            StorageSpaceId = batch.StorageSpaceId,
            NewQuantity = batch.Quantity,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory OriginalBatchPartialDiscard(Batch batch, Batch discardedBatch, int userId,
        double previousQuantity, int previousFoodUoMId)
    {
        return new BatchModificationHistory
        {
            BatchId = batch.Id,
            ModificationType = BatchModificationType.PartialDiscard,
            StorageSpaceId = batch.StorageSpaceId,
            PreviousQuantity = previousQuantity,
            DeltaQuantity = discardedBatch.Quantity,
            NewQuantity = batch.Quantity,
            PreviousFoodUoMId = previousFoodUoMId,
            DeltaFoodUoMId = discardedBatch.FoodUoMId,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate,
            CreatedNewBatch = discardedBatch,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static BatchModificationHistory DiscardedBatchPartialDiscard(Batch discardedBatch, Batch originalBatch,
        int userId)
    {
        return new BatchModificationHistory
        {
            Batch = discardedBatch,
            ModificationType = BatchModificationType.PartialDiscard,
            StorageSpaceId = discardedBatch.StorageSpaceId,
            NewQuantity = discardedBatch.Quantity,
            NewFoodUoMId = discardedBatch.FoodUoMId,
            BestBeforeDate = discardedBatch.BestBeforeDate,
            ExpirationDate = discardedBatch.ExpirationDate,
            CreatedFromBatch = originalBatch,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BatchModificationHistory>()
            .Property(f => f.ModificationType)
            .HasConversion(new BatchModificationTypeConverter());
    }
}