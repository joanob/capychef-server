using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdRealtimeService
{
    Task SendJoinRequestReceivedMessage(HouseholdJoinRequest joinRequest, int ownerUserId);
    Task SendMemberJoinedMessage(int householdId, int newMemberId);
    Task SendMemberRemovedMessage(int removedUserId, int householdId);
    Task SendHouseholdDeletedMessage(int householdId);
}