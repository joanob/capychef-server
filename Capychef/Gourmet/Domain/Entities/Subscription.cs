using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Gourmet.Domain.Entities;

[Table("subscriptions")]
public class Subscription
{
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

    [Column("id")] public int Id { get; private set; }

    [Column("user_id")] public int UserId { get; private set; }

    [Column("purchased_at")] public DateTime PurchasedAt { get; private set; }

    [Column("valid_from")] public DateTime ValidFrom { get; private set; }

    [Column("expires_at")] public DateTime ExpiresAt { get; private set; }

    [Column("subscription_type")] public SubscriptionType SubscriptionType { get; private set; }

    [Column("automatic_renewal")] public bool AutomaticRenewal { get; set; }

    [Column("is_primary_subscription")] public bool IsPrimarySubscription { get; set; }

    [Column("primary_subscription_id")] public int? PrimarySubscriptionId { get; private set; }

    [Column("household_id")] public int? HouseholdId { get; private set; }

    [Column("discount_id")] public int? DiscountId { get; private set; }

    [Column("amount_payed")] public double AmountPayed { get; private set; }

    [Column("amount_saved")] public double AmountSaved { get; private set; }

    [ForeignKey(nameof(UserId))] public User User { get; private set; } = null!;

    [ForeignKey(nameof(PrimarySubscriptionId))]
    public Subscription? PrimarySubscription { get; private set; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; private set; }

    [ForeignKey(nameof(DiscountId))] public SubscriptionDiscount? SubscriptionDiscount { get; private set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>()
            .Property(f => f.SubscriptionType)
            .HasConversion(new SubscriptionTypeConverter());
    }
}