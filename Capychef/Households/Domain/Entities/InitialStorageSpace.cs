using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Domain.Entities;

[Table("initial_storage_spaces")]
public class InitialStorageSpace
{
    public InitialStorageSpace(int id, string name, StorageConditions storageCondition)
    {
        Id = id;
        Name = name;
        StorageCondition = storageCondition;
    }

    public InitialStorageSpace(string name, StorageConditions storageCondition)
    {
        Name = name;
        StorageCondition = storageCondition;
    }

    [Column("id")] public int Id { get; private set; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("storage_condition")] public StorageConditions StorageCondition { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InitialStorageSpace>()
            .Property(f => f.StorageCondition)
            .HasConversion(new StorageConditionsConverter());
    }
}