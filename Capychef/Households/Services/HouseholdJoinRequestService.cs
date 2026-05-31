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
using Microsoft.EntityFrameworkCore;

namespace Capychef.Households.Services;

public class HouseholdJoinRequestService(
    CapychefDbContext dbContext,
    IHouseholdJoinRequestRepository joinRequestRepository,
    IHouseholdMemberRepository householdMemberRepository,
    IHouseholdRepository householdRepository,
    IUserRepository userRepository,
    ISubscriptionService subscriptionService,
    IMembershipCache membershipCache,
    IHouseholdRealtimeService householdRealtimeService) : IHouseholdJoinRequestService
{
    public async Task<AppError?> CreateJoinRequest(AuthUserDetails userDetails, CreateHouseholdJoinRequestCmd cmd)
    {
        var error = await subscriptionService.CheckUserCanBecomeHouseholdMember(userDetails.UserId);
        if (error != null) return error;

        var household = await householdRepository.GetTrackedHouseholdByPublicId(cmd.HouseholdPublicId);
        if (household == null) return new NotFoundError(EntityType.Household, cmd.HouseholdPublicId);

        if (await householdMemberRepository.CheckHouseholdMembership(userDetails.UserId, household.Id))
            return new ValidationError("User is already a member of this household");

        if (await joinRequestRepository.CheckPendingJoinRequestExistsByHouseholdIdAndUserId(household.Id,
                userDetails.UserId))
            return new ValidationError("User already has a pending join request for this household");

        var user = await userRepository.GetTrackedUserById(userDetails.UserId);
        if (user == null) return new NotFoundError(EntityType.User, userDetails.UserId);

        var joinRequest = new HouseholdJoinRequest(household, user);

        await joinRequestRepository.AddJoinRequestAsync(joinRequest);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ConcurrencyError();
        }

        _ = householdRealtimeService.SendJoinRequestReceivedMessage(joinRequest, household.OwnerId);

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

        joinRequest.Accept();

        var member = new HouseholdMember(joinRequest.HouseholdId, joinRequest.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ConcurrencyError();
        }

        await membershipCache.InvalidateAsync(joinRequest.UserId, joinRequest.HouseholdId);

        _ = householdRealtimeService.SendMemberJoinedMessage(joinRequest.HouseholdId, joinRequest.UserId);

        return null;
    }

    public async Task<AppError?> RejectJoinRequest(AuthUserDetails userDetails, int joinRequestId)
    {
        var joinRequest =
            await joinRequestRepository.GetTrackedJoinRequestById(joinRequestId, userDetails.GetHouseholdId());

        if (joinRequest == null || joinRequest.IsAnswered)
            return new NotFoundError(EntityType.HouseholdJoinRequest, joinRequestId);

        joinRequest.Reject();

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ConcurrencyError();
        }

        return null;
    }
}