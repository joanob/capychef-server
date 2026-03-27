using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Common.Entities;

public class BaseEntity
{
    [Column("id")] public int Id { get; init; }

    [Column("created_at")] public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    [Column("row_version")] public int RowVersion { get; init; }
}