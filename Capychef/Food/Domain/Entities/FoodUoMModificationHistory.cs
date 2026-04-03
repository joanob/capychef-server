using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Domain.Entities;

[Table("food_uom_modifications_history")]
public class FoodUoMModificationHistory
{
    protected  FoodUoMModificationHistory() {}
    
    public FoodUoMModificationHistory(int foodId, int householdId, FoodModifiableColumn columnName, string previousValue,
        string newValue,
        int modifiedBy)
    {
        FoodId = foodId;
        HouseholdId = householdId;
        ColumnName = columnName;
        PreviousValue = previousValue;
        NewValue = newValue;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    [Column("id")] public int Id { get; init; }

    [Column("food_uom_id")] public int FoodUoMId { get; init; }
    
    [Column("column_name")] public FoodModifiableColumn ColumnName { get; init; }

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