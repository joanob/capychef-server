using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Households.Domain.Entities;

[Table("initial_storage_spaces")]
public class InitialStorageSpace
{
    public InitialStorageSpace()
    {
    }

    public InitialStorageSpace(string name, StorageConditions storageCondition)
    {
        Name = name;
        StorageCondition = storageCondition;
    }

    [Column("id")] public int Id { get; private set; }

    [Column("name")] public string Name { get; set; }

    [Column("storage_condition")] public StorageConditions StorageCondition { get; set; }
}