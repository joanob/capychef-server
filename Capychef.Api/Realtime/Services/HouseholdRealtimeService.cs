using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Capychef.Api.Realtime.Services;

public class HouseholdRealtimeService(IConnectionMappingStore connections, IHubContext<RealtimeHub> hubContext)
    : IHouseholdRealtimeService
{
    public async Task SendJoinRequestReceivedMessage(HouseholdJoinRequest joinRequest, int ownerUserId)
    {
        var payload = new { joinRequest.HouseholdId, joinRequest.UserId };

        await SendToUser(ownerUserId, "HouseholdJoinRequest.Received", payload);
    }

    public async Task SendMemberJoinedMessage(int householdId, int newMemberId)
    {
        var payload = new { HouseholdId = householdId, UserId = newMemberId };

        await SendToHousehold(householdId, "Household.MemberJoined", payload);
    }

    public async Task SendMemberRemovedMessage(int removedUserId, int householdId)
    {
        var payload = new { HouseholdId = householdId };

        await SendToUser(removedUserId, "Household.MemberRemoved", payload);
    }

    public async Task SendHouseholdDeletedMessage(int householdId)
    {
        var payload = new { HouseholdId = householdId };

        await SendToHousehold(householdId, "Household.Deleted", payload);
    }

    private async Task SendToUser(int userId, string method, object payload)
    {
        var connectionIds = (await connections.GetByUserAsync(userId))
            .Select(c => c.ConnectionId)
            .ToList();

        if (!connectionIds.Any()) return;

        await hubContext.Clients.Clients(connectionIds).SendAsync(method, payload);
    }

    private async Task SendToHousehold(int householdId, string method, object payload)
    {
        var connectionIds = (await connections.GetByHouseholdAsync(householdId))
            .Select(c => c.ConnectionId)
            .ToList();

        if (!connectionIds.Any()) return;

        await hubContext.Clients.Clients(connectionIds).SendAsync(method, payload);
    }
}