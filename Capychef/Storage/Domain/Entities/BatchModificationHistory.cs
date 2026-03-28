using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Entities;
using Capychef.Food.Domain.Entities;
using Capychef.Households.Domain.Entities;

namespace Capychef.Storage.Domain.Entities;

[Table("batch_modifications_history")]
public class BatchModificationHistory : BaseEntity
{
    [Column("batch_id")] public int BatchId { get; init; }

    [Column("storage_space_id")] public int StorageSpaceId { get; set; }

    [Column("quantity")] public double Quantity { get; set; }

    [Column("food_uom_id")] public int FoodUoMId { get; set; }

    [Column("best_before_date")] public DateTime? BestBeforeDate { get; set; }

    [Column("expiration_date")] public DateTime? ExpirationDate { get; set; }

    [ForeignKey(nameof(BatchId))] public Batch? Batch { get; set; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace? StorageSpace { get; set; }

    [ForeignKey(nameof(FoodUoMId))] public FoodUoM? FoodUoM { get; set; }
}