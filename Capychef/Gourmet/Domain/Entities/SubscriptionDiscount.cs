using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Gourmet.Domain.Entities;

[Table("discounts")]
public class SubscriptionDiscount
{
    public SubscriptionDiscount()
    {
    }

    public SubscriptionDiscount(string discountCode, DiscountType discountType, double percentageOrAmount,
        DateTime validFrom, DateTime validUntil)
    {
        Name = discountCode;
        DiscountType = discountType;
        PercentageOrAmount = percentageOrAmount;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    [Column("id")] public int Id { get; }

    [Column("discount_code")] public string Name { get; }

    [Column("discount_type")] public DiscountType DiscountType { get; }

    [Column("percentage_or_amount")] public double PercentageOrAmount { get; }

    [Column("valid_from")] public DateTime ValidFrom { get; set; }

    [Column("valid_until")] public DateTime ValidUntil { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriptionDiscount>()
            .Property(f => f.DiscountType)
            .HasConversion(new DiscountTypeConverter());
    }
}