using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Shopping.Domain.Entities;

[Table("supermarkets")]
public class Supermarket
{
    protected Supermarket()
    {
        Name = "";
    }

    [Column("id")] public int Id { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; set; }

    [Column("is_global")] public bool IsGlobal { get; init; }

    [Column("global_id")] [MaxLength(50)] public string? GlobalId { get; init; }

    [Column("household_id")] public int? HouseholdId { get; init; }

    [Column("modified_global_supermarket_id")]
    public int? ModifiedGlobalFoodId { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("created_by")] public int CreatedBy { get; init; }

    [Column("row_version")] public int RowVersion { get; init; }

    [Column("is_deleted")] public bool IsDeleted { get; private set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; private set; }

    public void Delete()
    {
        if (!IsGlobal)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}

public static class SupermarketExtensions
{
    public static IQueryable<Supermarket> Active(this IQueryable<Supermarket> supermarkets)
    {
        return supermarkets.Where(x => !x.IsDeleted);
    }
}