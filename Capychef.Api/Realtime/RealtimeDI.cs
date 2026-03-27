using Capychef.Api.Realtime.Services;
using Capychef.Households.Domain.Interfaces;

namespace Capychef.Api.Realtime;

public static class RealtimeDi
{
    public static IServiceCollection AddRealtimeDi(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMappingStore, InMemoryConnectionMappingStore>();
        services.AddSingleton<IHouseholdInvitationRealtimeService, HouseholdInvitationRealtimeService>();

        return services;
    }
}