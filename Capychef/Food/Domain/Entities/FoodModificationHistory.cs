using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Food.Domain.Entities;

[Table("food_modifications_history")]
public class FoodModificationHistory
{
    public FoodModificationHistory(int foodId, int householdId, FoodModifiableColumn columnName, string previousValue,
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

    [Column("food_id")] public int FoodId { get; init; }

    [Column("household_id")] public int HouseholdId { get; init; }

    [Column("column_name")] public FoodModifiableColumn ColumnName { get; init; }

    [Column("previous_value")]
    [MaxLength(100)]
    public string PreviousValue { get; init; }

    [Column("new_value")] [MaxLength(100)] public string NewValue { get; init; }

    [Column("modified_at")] public DateTime ModifiedAt { get; init; }

    [Column("modified_by")] public int ModifiedBy { get; init; }

    [InverseProperty(nameof(FoodId))] public Food? Food { get; init; }
    [InverseProperty(nameof(ModifiedBy))] public User? ModifiedByUser { get; init; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodModificationHistory>()
            .Property(f => f.ColumnName)
            .HasConversion(new FoodModifiableColumnConverter());
    }
}