﻿// csharp
namespace Capychef.Api.Realtime;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class InMemoryConnectionMappingStore : IConnectionMappingStore
{
    private readonly ConcurrentDictionary<string, ConnectionDetails> _connections;

    public InMemoryConnectionMappingStore()
    {
        _connections = new ConcurrentDictionary<string, ConnectionDetails>();
    }
    
    public Task AddAsync(ConnectionDetails connection)
    {
        _connections[connection.ConnectionId] = connection;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<ConnectionDetails>> GetByUserAsync(int userId)
    {
        return Task.FromResult(_connections.Values.Where(c => c.UserId == userId));
    }

    public Task<IEnumerable<ConnectionDetails>> GetByHouseholdAsync(int householdId)
    {
        return Task.FromResult(_connections.Values.Where(c => c.HouseholdId == householdId));
    }

    public Task<IEnumerable<ConnectionDetails>> GetByUserAndHouseholdAsync(int userId, int householdId)
    {
        return Task.FromResult(_connections.Values.Where(c => c.UserId == userId && c.HouseholdId == householdId));
    }

    public Task UpdateHouseholdAsync(string connectionId, int? householdId)
    {
        if (_connections.TryGetValue(connectionId, out var connection))
        {
            connection.UpdateHouseholdId(householdId);
        }
        
        return Task.CompletedTask;
    }
}