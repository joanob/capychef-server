using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Capychef.Api.Realtime.Services;

public class HouseholdInvitationRealtimeService : IHouseholdInvitationRealtimeService
{
    private readonly IConnectionMappingStore _connections;
    private readonly IHubContext<RealtimeHub> _hubContext;

    public HouseholdInvitationRealtimeService(IConnectionMappingStore connections, IHubContext<RealtimeHub> hubContext)
    {
        _connections = connections;
        _hubContext = hubContext;
    }

    public async Task SendHouseholdInvitationReceivedMessage(HouseholdInvitation householdInvitation)
    {
        var payload = new
        {
            HouseholdId = householdInvitation.Household.Id,
            HouseholdName = householdInvitation.Household.Name
        };

        var connections = await _connections.GetByUserAsync(householdInvitation.UserId);

        var connectionIds = connections.Select(c => c.ConnectionId).ToList();

        if (!connectionIds.Any()) return;

        foreach (var connectionId in connectionIds)
            await _hubContext.Clients.Client(connectionId).SendAsync("HouseholdInvitation.Received", payload);
    }
}