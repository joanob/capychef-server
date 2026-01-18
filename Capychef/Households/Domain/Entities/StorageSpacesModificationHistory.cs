using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using YourOwnBoss.Common.Entities;

namespace Capychef.Food.Domain.Entities;

[Table("storage_spaces_modifications_history")]
public class StorageSpacesModificationHistory
{
    public StorageSpacesModificationHistory()
    {
    }

    public StorageSpacesModificationHistory(int storageSpaceId, StorageSpaceModifiableColumn columnName,
        string previousValue,
        string newValue,
        int modifiedBy)
    {
        StorageSpaceId = storageSpaceId;
        ColumnName = columnName;
        PreviousValue = previousValue;
        NewValue = newValue;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    [Column("id")] public int Id { get; private set; }

    [Column("storage_space_id")] public int StorageSpaceId { get; private set; }

    [Column("column_name")] public StorageSpaceModifiableColumn ColumnName { get; private set; }

    [Column("previous_value")] public string PreviousValue { get; private set; }

    [Column("new_value")] public string NewValue { get; private set; }

    [Column("modified_at")] public DateTime ModifiedAt { get; private set; }

    [Column("modified_by")] public int ModifiedBy { get; private set; }

    [ForeignKey(nameof(StorageSpaceId))] public StorageSpace StorageSpace { get; private set; }

    [ForeignKey(nameof(ModifiedBy))] public User ModifiedByUser { get; private set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageSpacesModificationHistory>()
            .Property(f => f.ColumnName)
            .HasConversion(new StorageSpaceModifiableColumnConverter());
    }
}