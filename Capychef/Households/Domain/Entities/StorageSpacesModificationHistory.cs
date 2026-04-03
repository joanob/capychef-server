using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Domain.Entities;

[Table("storage_spaces_modifications_history")]
public class StorageSpacesModificationHistory
{
    protected StorageSpacesModificationHistory() { }
    
    public StorageSpacesModificationHistory(int storageSpaceId, StorageSpaceModifiableColumn columnName,
        string previousValue,
        string newValue,
        int userId)
    {
        StorageSpaceId = storageSpaceId;
        ColumnName = columnName;
        PreviousValue = previousValue;
        NewValue = newValue;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = userId;
    }

    [Column("id")] public int Id { get; init; }

    [Column("storage_space_id")] public int StorageSpaceId { get; init; }

    [Column("column_name")] public StorageSpaceModifiableColumn ColumnName { get; init; }

    [Column("previous_value")]
    [MaxLength(50)]
    public string PreviousValue { get; init; }

    [Column("new_value")] [MaxLength(50)] public string NewValue { get; init; }

    [Column("modified_at")] public DateTime ModifiedAt { get; init; }

    [Column("modified_by")] public int ModifiedBy { get; init; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace? StorageSpace { get; init; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageSpacesModificationHistory>()
            .Property(f => f.ColumnName)
            .HasConversion(new StorageSpaceModifiableColumnConverter());
    }
}