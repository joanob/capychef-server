using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdInvitationRealtimeService
{
    Task SendHouseholdInvitationReceivedMessage(HouseholdInvitation householdInvitation);
}