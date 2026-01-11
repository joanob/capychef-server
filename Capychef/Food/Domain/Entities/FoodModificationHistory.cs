using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using YourOwnBoss.Common.Entities;

namespace Capychef.Food.Domain.Entities;

[Table("food_modifications_history")]
public class FoodModificationHistory
{
    public FoodModificationHistory()
    {
    }

    public FoodModificationHistory(int foodId, FoodModifiableColumn columnName, string previousValue, string newValue,
        int modifiedBy)
    {
        FoodId = foodId;
        ColumnName = columnName;
        PreviousValue = previousValue;
        NewValue = newValue;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    [Column("id")] public int Id { get; private set; }

    [Column("food_id")]
    [ForeignKey(nameof(Food))]
    public int FoodId { get; private set; }

    [Column("column_name")] public FoodModifiableColumn ColumnName { get; private set; }

    [Column("previous_value")] public string PreviousValue { get; private set; }

    [Column("new_value")] public string NewValue { get; private set; }

    [Column("modified_at")] public DateTime ModifiedAt { get; private set; }

    [Column("modified_by")]
    [ForeignKey(nameof(ModifiedByUser))]
    public int ModifiedBy { get; private set; }

    public Food Food { get; private set; }
    public User ModifiedByUser { get; private set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodModificationHistory>()
            .Property(f => f.ColumnName)
            .HasConversion(new FoodModifiableColumnConverter());
    }
}