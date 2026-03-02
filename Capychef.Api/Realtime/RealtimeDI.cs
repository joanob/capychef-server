using Capychef.Api.Realtime.Services;
using Capychef.Households.Domain.Interfaces;

namespace Capychef.Api.Realtime;

public static class RealtimeDI
{
    public static IServiceCollection AddRealtimeDI(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMappingStore, InMemoryConnectionMappingStore>();
        services.AddSingleton<IHouseholdInvitationRealtimeService, HouseholdInvitationRealtimeService>();
        
        return services;
    }
}