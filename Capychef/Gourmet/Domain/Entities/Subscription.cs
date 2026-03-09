using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Common.Errors;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Gourmet.Domain.Entities;

[Table("subscriptions")]
public class Subscription
{
    public Subscription()
    {
    }

    public Subscription(int userId, DateTime purchasedAt, DateTime validFrom, DateTime expiresAt,
        SubscriptionType subscriptionType, bool automaticRenewal, bool isPrimarySubscription,
        int? primarySubscriptionId, int? householdId, int? discountId, double amountPayed, double amountSaved)
    {
        UserId = userId;
        PurchasedAt = purchasedAt;
        ValidFrom = validFrom;
        ExpiresAt = expiresAt;
        SubscriptionType = subscriptionType;
        AutomaticRenewal = automaticRenewal;
        IsPrimarySubscription = isPrimarySubscription;
        PrimarySubscriptionId = primarySubscriptionId;
        HouseholdId = householdId;
        DiscountId = discountId;
        AmountPayed = amountPayed;
        AmountSaved = amountSaved;
    }

    [Column("id")] public int Id { get; }

    [Column("user_id")] public int UserId { get; }

    [Column("purchased_at")] public DateTime PurchasedAt { get; }

    [Column("valid_from")] public DateTime ValidFrom { get; }

    [Column("expires_at")] public DateTime ExpiresAt { get; }

    [Column("subscription_type")] public SubscriptionType SubscriptionType { get; }

    [Column("automatic_renewal")] public bool AutomaticRenewal { get; set; }

    [Column("is_primary_subscription")] public bool IsPrimarySubscription { get; set; }

    [Column("primary_subscription_id")] public int? PrimarySubscriptionId { get; }

    [Column("household_id")] public int? HouseholdId { get; }

    [Column("discount_id")] public int? DiscountId { get; }

    [Column("amount_payed")] public double AmountPayed { get; }

    [Column("amount_saved")] public double AmountSaved { get; }

    [ForeignKey(nameof(UserId))] public User User { get; }

    [ForeignKey(nameof(PrimarySubscriptionId))]
    public Subscription? PrimarySubscription { get; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; }

    [ForeignKey(nameof(DiscountId))] public SubscriptionDiscount? SubscriptionDiscount { get; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>()
            .Property(f => f.SubscriptionType)
            .HasConversion(new SubscriptionTypeConverter());
    }
}