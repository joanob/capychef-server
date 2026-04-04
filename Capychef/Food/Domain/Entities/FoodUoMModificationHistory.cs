using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Domain.Entities;

[Table("food_uom_modifications_history")]
public class FoodUoMModificationHistory
{
    protected FoodUoMModificationHistory()
    {
        ColumnName = FoodUoMModifiableColumn.From("");
        PreviousValue = "";
        NewValue = "";
    }

    public FoodUoMModificationHistory(int foodUoMId, int householdId, FoodUoMModifiableColumn columnName,
        string previousValue,
        string newValue,
        int modifiedBy)
    {
        FoodUoMId = foodUoMId;
        ColumnName = columnName;
        PreviousValue = previousValue;
        NewValue = newValue;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    [Column("id")] public int Id { get; init; }

    [Column("food_uom_id")] public int FoodUoMId { get; init; }

    [Column("column_name")] public FoodUoMModifiableColumn ColumnName { get; init; }

    [Column("previous_value")]
    [MaxLength(100)]
    public string PreviousValue { get; init; }

    [Column("new_value")] [MaxLength(100)] public string NewValue { get; init; }

    [Column("modified_at")] public DateTime ModifiedAt { get; init; }

    [Column("modified_by")] public int ModifiedBy { get; init; }

    [ForeignKey(nameof(FoodUoMId))] public FoodUoM? Food { get; init; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodUoMModificationHistory>()
            .Property(f => f.ColumnName)
            .HasConversion(new FoodUoMModifiableColumnConverter());
    }
}