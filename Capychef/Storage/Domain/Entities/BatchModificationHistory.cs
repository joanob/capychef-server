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

    [Column("previous_quantity")] public double PreviousQuantity { get; init; }

    [Column("delta_quantity")] public double DeltaQuantity { get; init; }

    [Column("new_quantity")] public double NewQuantity { get; init; }

    [Column("previous_food_uom_id")] public int PreviousFoodUoMId { get; init; }

    [Column("delta_food_uom_id")] public int DeltaFoodUoMId { get; init; }

    [Column("new_food_uom_id")] public int NewFoodUoMId { get; init; }

    [Column("best_before_date")] public DateTime? BestBeforeDate { get; init; }

    [Column("expiration_date")] public DateTime? ExpirationDate { get; init; }

    [ForeignKey(nameof(BatchId))] public Batch? Batch { get; init; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace? StorageSpace { get; init; }

    [ForeignKey(nameof(PreviousFoodUoMId))]
    public FoodUoM? PreviousFoodUoM { get; init; }

    [ForeignKey(nameof(DeltaFoodUoMId))] public FoodUoM? DeltaFoodUoM { get; init; }

    [ForeignKey(nameof(NewFoodUoMId))] public FoodUoM? NewFoodUoM { get; init; }

    public static BatchModificationHistory FromInitial(Batch batch)
    {
        return new BatchModificationHistory
        {
            BatchId = batch.Id,
            ModificationType = BatchModificationType.Initial,
            StorageSpaceId = batch.StorageSpaceId,
            PreviousQuantity = batch.Quantity,
            DeltaQuantity = 0, // initial history entry
            NewQuantity = batch.Quantity,
            PreviousFoodUoMId = batch.FoodUoMId,
            DeltaFoodUoMId = batch.FoodUoMId,
            NewFoodUoMId = batch.FoodUoMId,
            BestBeforeDate = batch.BestBeforeDate,
            ExpirationDate = batch.ExpirationDate
        };
    }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BatchModificationHistory>()
            .Property(f => f.ModificationType)
            .HasConversion(new BatchModificationTypeConverter());
    }
}