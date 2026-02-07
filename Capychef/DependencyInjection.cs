using Capychef.Food.Domain.Interfaces;
using Capychef.Food.Repositories;
using Capychef.Food.Services;
using Capychef.Households.Domain.Interfaces;
using Capychef.Households.Repositories;
using Capychef.Households.Services;
using Capychef.Infrastructure.DevImplementations;
using Capychef.Infrastructure.Interfaces;
using Capychef.Persistence;
using Capychef.Storage;
using Capychef.Storage.Domain.Interfaces;
using Capychef.Storage.Services;
using Capychef.Testdata;
using Capychef.Users.Domain.Interfaces;
using Capychef.Users.Repositories;
using Capychef.Users.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Capychef;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        // DATABASE

        Env.Load("../.env");

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        services.AddDbContext<CapychefDbContext>(options =>
            options.UseNpgsql(connectionString));

        // EMAIL

        services.AddScoped<IEmailSender, Smtp4DevSender>();

        // HEALTH

        services.AddScoped<HealthCheckService, HealthCheckService>();

        // USERS

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserPasswordRepository, UserPasswordRepository>();
        services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserSessionService, UserSessionService>();

        // HOUSEHOLD 

        services.AddScoped<IHouseholdRepository, HouseholdRepository>();
        services.AddScoped<IHouseholdMemberRepository, HouseholdMemberRepository>();
        services.AddScoped<IHouseholdInvitationRepository, HouseholdInvitationRepository>();
        services.AddScoped<IHouseholdJoinRequestRepository, HouseholdJoinRequestRepository>();
        services.AddScoped<IStorageSpaceRepository, StorageSpaceRepository>();
        services.AddScoped<IStorageSpaceModificationHistoryRepository, StorageSpaceModificationHistoryRepository>();
        services.AddScoped<IHouseholdService, HouseholdService>();
        services.AddScoped<IHouseholdMemberService, HouseholdMemberService>();
        services.AddScoped<IHouseholdInvitationService, HouseholdInvitationService>();
        services.AddScoped<IHouseholdJoinRequestService, HouseholdJoinRequestService>();
        services.AddScoped<IStorageSpaceService, StorageSpaceService>();

        // FOOD 

        services.AddScoped<IUoMDimensionRepository, UoMDimensionRepository>();
        services.AddScoped<IUoMRepository, UoMRepository>();
        services.AddScoped<IUoMService, UoMService>();
        services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
        services.AddScoped<IFoodCategoryService, FoodCategoryService>();
        services.AddScoped<IFoodRepository, FoodRepository>();
        services.AddScoped<IFoodUoMRepository, FoodUoMRepository>();
        services.AddScoped<IFoodModificationHistoryRepository, FoodModificationHistoryRepository>();
        services.AddScoped<IFoodService, FoodService>();

        // STORAGE
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IBatchService, BatchService>();

        // TESTDATA

        services.AddScoped<TestUsers, TestUsers>();
        services.AddScoped<TestHouseholds, TestHouseholds>();
        services.AddScoped<TestFood, TestFood>();
        services.AddScoped<Testdata.Testdata, Testdata.Testdata>();

        return services;
    }
}