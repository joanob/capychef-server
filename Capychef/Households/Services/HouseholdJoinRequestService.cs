using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Interfaces;
using YourOwnBoss.Common.Entities;
using YourOwnBoss.Common.Errors;

namespace Capychef.Households.Services;

public class HouseholdJoinRequestService(
    CapychefDbContext dbContext,
    IHouseholdJoinRequestRepository joinRequestRepository,
    IHouseholdMemberRepository householdMemberRepository,
    IHouseholdRepository householdRepository,
    IUserRepository userRepository) : IHouseholdJoinRequestService
{
    public async Task<AppError?> CreateJoinRequest(AuthUserDetails userDetails, CreateHouseholdJoinRequestCmd cmd)
    {
        var household = await householdRepository.GetTrackedHouseholdByPublicId(cmd.HouseholdPublicId);
        if (household == null) return new NotFoundError(EntityType.Household, cmd.HouseholdPublicId);

        var user = await userRepository.GetTrackedUserById(userDetails.UserId);
        if (user == null) return new NotFoundError(EntityType.User, userDetails.UserId);

        var joinRequest = new HouseholdJoinRequest(household, user);

        await joinRequestRepository.AddJoinRequestAsync(joinRequest);

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<List<HouseholdJoinRequestDTO>> GetAllHouseholdJoinRequests(AuthUserDetails userDetails)
    {
        var joinRequests = await joinRequestRepository.GetHouseholdJoinRequests(userDetails.HouseholdId.Value);

        return HouseholdJoinRequestDTO.toList(joinRequests);
    }

    public async Task<List<HouseholdJoinRequestDTO>> GetHouseholdJoinRequestsByUser(AuthUserDetails userDetails)
    {
        var joinRequests = await joinRequestRepository.GetHouseholdJoinRequestsByUserId(userDetails.UserId);

        return HouseholdJoinRequestDTO.toList(joinRequests);
    }

    public async Task<AppError?> AcceptJoinRequest(AuthUserDetails userDetails, int joinRequestId)
    {
        var joinRequest =
            await joinRequestRepository.GetTrackedJoinRequestById(joinRequestId, userDetails.HouseholdId.Value);

        if (joinRequest == null || joinRequest.IsAnswered)
            return new NotFoundError(EntityType.HouseholdJoinRequest, joinRequestId);

        joinRequest.IsAnswered = true;
        joinRequest.AnsweredAt = DateTime.Now;
        joinRequest.IsAccepted = true;

        var member = new HouseholdMember(joinRequest.HouseholdId, joinRequest.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> RejectJoinRequest(AuthUserDetails userDetails, int joinRequestId)
    {
        var joinRequest =
            await joinRequestRepository.GetTrackedJoinRequestById(joinRequestId, userDetails.HouseholdId.Value);

        if (joinRequest == null || joinRequest.IsAnswered)
            return new NotFoundError(EntityType.HouseholdJoinRequest, joinRequestId);

        joinRequest.IsAnswered = true;
        joinRequest.AnsweredAt = DateTime.Now;
        joinRequest.IsAccepted = false;

        await dbContext.SaveChangesAsync();

        return null;
    }
}