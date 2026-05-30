using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Gourmet.Domain.Interfaces;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Households.Services;

public class HouseholdJoinRequestService(
    CapychefDbContext dbContext,
    IHouseholdJoinRequestRepository joinRequestRepository,
    IHouseholdMemberRepository householdMemberRepository,
    IHouseholdRepository householdRepository,
    IUserRepository userRepository,
    ISubscriptionService subscriptionService) : IHouseholdJoinRequestService
{
    public async Task<AppError?> CreateJoinRequest(AuthUserDetails userDetails, CreateHouseholdJoinRequestCmd cmd)
    {
        var error = await subscriptionService.CheckUserCanBecomeHouseholdMember(userDetails.UserId);
        if (error != null) return error;

        var household = await householdRepository.GetTrackedHouseholdByPublicId(cmd.HouseholdPublicId);
        if (household == null) return new NotFoundError(EntityType.Household, cmd.HouseholdPublicId);

        var user = await userRepository.GetTrackedUserById(userDetails.UserId);
        if (user == null) return new NotFoundError(EntityType.User, userDetails.UserId);

        var joinRequest = new HouseholdJoinRequest(household, user);

        await joinRequestRepository.AddJoinRequestAsync(joinRequest);

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<List<HouseholdJoinRequestDto>> GetAllHouseholdJoinRequests(AuthUserDetails userDetails)
    {
        var joinRequests = await joinRequestRepository.GetHouseholdJoinRequests(userDetails.GetHouseholdId());

        return HouseholdJoinRequestDto.ToList(joinRequests);
    }

    public async Task<List<HouseholdJoinRequestDto>> GetHouseholdJoinRequestsByUser(AuthUserDetails userDetails)
    {
        var joinRequests = await joinRequestRepository.GetHouseholdJoinRequestsByUserId(userDetails.UserId);

        return HouseholdJoinRequestDto.ToList(joinRequests);
    }

    public async Task<AppError?> AcceptJoinRequest(AuthUserDetails userDetails, int joinRequestId)
    {
        var joinRequest =
            await joinRequestRepository.GetTrackedJoinRequestById(joinRequestId, userDetails.GetHouseholdId());

        if (joinRequest == null || joinRequest.IsAnswered)
            return new NotFoundError(EntityType.HouseholdJoinRequest, joinRequestId);

        var error = await subscriptionService.CheckUserCanBecomeHouseholdMember(joinRequest.UserId);
        if (error != null) return error;

        joinRequest.IsAnswered = true;
        joinRequest.AnsweredAt = DateTime.UtcNow;
        joinRequest.IsAccepted = true;

        var member = new HouseholdMember(joinRequest.HouseholdId, joinRequest.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> RejectJoinRequest(AuthUserDetails userDetails, int joinRequestId)
    {
        var joinRequest =
            await joinRequestRepository.GetTrackedJoinRequestById(joinRequestId, userDetails.GetHouseholdId());

        if (joinRequest == null || joinRequest.IsAnswered)
            return new NotFoundError(EntityType.HouseholdJoinRequest, joinRequestId);

        joinRequest.IsAnswered = true;
        joinRequest.AnsweredAt = DateTime.UtcNow;
        joinRequest.IsAccepted = false;

        await dbContext.SaveChangesAsync();

        return null;
    }
}