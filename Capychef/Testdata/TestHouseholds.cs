using Capychef.Common.Utils;
using Capychef.Households.Domain.Entities;
using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Testdata;

public class TestHouseholds(CapychefDbContext dbContext)
{
    private readonly float DELETED_HOUSEHOLD_INVITATIONS_PERCENT = 0.2f;
    private readonly float DELETED_HOUSEHOLD_JOIN_REQUESTS_PERCENT = 0.2f;
    private readonly float DELETED_HOUSEHOLD_MEMBERS_PERCENT = 0.2f;
    private readonly float DELETED_HOUSEHOLDS_PERCENTAJE = 0.2f;
    private readonly List<Household> households = new();
    private readonly int HOUSEHOLDS = 200;
    private readonly int MAX_HOUSEHOLD_INVITATIONS = 10;
    private readonly int MAX_HOUSEHOLD_JOIN_REQUESTS = 10;
    private readonly int MAX_HOUSEHOLD_MEMBERS = 5;
    private readonly int MIN_HOUSEHOLD_INVITATIONS = 0;
    private readonly int MIN_HOUSEHOLD_JOIN_REQUESTS = 0;
    private readonly int MIN_HOUSEHOLD_MEMBERS = 2;

    private List<User> users = new();

    public async Task Generate()
    {
        users.Clear();
        households.Clear();

        users = await dbContext.Users.ToListAsync();

        await GenerateHouseholds();

        await dbContext.SaveChangesAsync();
    }

    private async Task GenerateHouseholds()
    {
        for (var i = 0; i < HOUSEHOLDS; i++)
        {
            var name = RandomGenerator.GenerateRandomAlphabetString(10);
            var owner = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));

            var household = new Household(owner, name);

            if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLDS_PERCENTAJE)) household.Delete();

            households.Add(household);

            await dbContext.AddAsync(household);

            var householdOwner = new HouseholdMember(household, owner);

            await dbContext.AddAsync(householdOwner);

            var householdMembers = new List<HouseholdMember>();
            var householdMembersNumber =
                RandomGenerator.GenerateRandomNumber(MAX_HOUSEHOLD_MEMBERS - MIN_HOUSEHOLD_MEMBERS) +
                MIN_HOUSEHOLD_MEMBERS;

            var householdInvitations = new List<HouseholdInvitation>();
            var householdInvitationsNumber =
                RandomGenerator.GenerateRandomNumber(MAX_HOUSEHOLD_INVITATIONS - MIN_HOUSEHOLD_INVITATIONS) +
                MIN_HOUSEHOLD_INVITATIONS;

            var householdJoinRequests = new List<HouseholdJoinRequest>();
            var householdJoinRequestsNumber =
                RandomGenerator.GenerateRandomNumber(MAX_HOUSEHOLD_JOIN_REQUESTS - MIN_HOUSEHOLD_JOIN_REQUESTS) +
                MIN_HOUSEHOLD_JOIN_REQUESTS;

            for (var j = 0; j < householdMembersNumber; j++)
            {
                var user = new User();
                do
                {
                    user = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));
                    if (!householdMembers.Any(h => h.UserId == user.Id)) break;
                } while (true);

                var householdMember = new HouseholdMember(household, user);

                if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLD_MEMBERS_PERCENT))
                    householdMember.Delete();

                householdMembers.Add(householdMember);

                await dbContext.AddAsync(householdMember);
            }

            for (var j = 0; j < householdInvitationsNumber; j++)
            {
                var user = new User();
                do
                {
                    user = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));
                    if (!householdMembers.Any(h => h.UserId == user.Id) &&
                        !householdInvitations.Any(h => h.UserId == user.Id)) break;
                } while (true);

                var householdInvitation = new HouseholdInvitation(household, user);

                if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLD_INVITATIONS_PERCENT))
                    householdInvitation.Delete();

                householdInvitations.Add(householdInvitation);

                await dbContext.AddAsync(householdInvitation);
            }

            for (var j = 0; j < householdJoinRequestsNumber; j++)
            {
                var user = new User();
                do
                {
                    user = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));
                    if (!householdMembers.Any(h => h.UserId == user.Id) &&
                        !householdInvitations.Any(h => h.UserId == user.Id) &&
                        !householdJoinRequests.Any(h => h.UserId == user.Id)) break;
                } while (true);

                var householdJoinRequest = new HouseholdJoinRequest(household, user);

                if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLD_JOIN_REQUESTS_PERCENT))
                    householdJoinRequest.Delete();

                householdJoinRequests.Add(householdJoinRequest);

                await dbContext.AddAsync(householdJoinRequest);
            }
        }
    }
}