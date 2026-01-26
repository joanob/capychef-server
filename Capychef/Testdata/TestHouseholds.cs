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
    private readonly float DELETED_STORAGE_SPACES_PERCENTAJE = 0.2f;
    private readonly int HOUSEHOLDS = 200;
    private readonly int MAX_HOUSEHOLD_INVITATIONS = 10;
    private readonly int MAX_HOUSEHOLD_JOIN_REQUESTS = 10;
    private readonly int MAX_HOUSEHOLD_MEMBERS = 5;
    private readonly int MAX_STORAGE_SPACES = 5;
    private readonly int MIN_HOUSEHOLD_INVITATIONS = 0;
    private readonly int MIN_HOUSEHOLD_JOIN_REQUESTS = 0;
    private readonly int MIN_HOUSEHOLD_MEMBERS = 2;
    private readonly int MIN_STORAGE_SPACES = 0;

    private List<User> users = new();

    public async Task Generate()
    {
        users = await dbContext.Users.ToListAsync();

        Console.WriteLine("Generating households");

        for (var i = 0; i < HOUSEHOLDS; i++)
        {
            var household = await GenerateHousehold();

            var members = await GenerateHouseholdMembers(household);

            var invitations = await GenerateHouseholdInvitations(household, members);

            await GenerateHouseholdJoinRequests(household, members, invitations);

            await GenerateHouseholdStorageSpaces(household, members);
        }

        Console.WriteLine("Households generated");

        await dbContext.SaveChangesAsync();
    }

    private async Task GenerateHouseholdJoinRequests(Household household, List<HouseholdMember> householdMembers,
        List<HouseholdInvitation> householdInvitations)
    {
        var householdJoinRequests = new List<HouseholdJoinRequest>();
        var householdJoinRequestsNumber =
            RandomGenerator.GenerateRandomNumber(MAX_HOUSEHOLD_JOIN_REQUESTS - MIN_HOUSEHOLD_JOIN_REQUESTS) +
            MIN_HOUSEHOLD_JOIN_REQUESTS;

        for (var j = 0; j < householdJoinRequestsNumber; j++)
        {
            var user = new User();
            do
            {
                user = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));
                if (householdMembers.All(h => h.UserId != user.Id) &&
                    householdInvitations.All(h => h.UserId != user.Id) &&
                    householdJoinRequests.All(h => h.UserId != user.Id)) break;
            } while (true);

            var householdJoinRequest = new HouseholdJoinRequest(household, user);

            if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLD_JOIN_REQUESTS_PERCENT))
                householdJoinRequest.Delete();

            householdJoinRequests.Add(householdJoinRequest);

            await dbContext.AddAsync(householdJoinRequest);
        }
    }

    private async Task<List<HouseholdInvitation>> GenerateHouseholdInvitations(Household household,
        List<HouseholdMember> householdMembers)
    {
        var householdInvitations = new List<HouseholdInvitation>();
        var householdInvitationsNumber =
            RandomGenerator.GenerateRandomNumber(MAX_HOUSEHOLD_INVITATIONS - MIN_HOUSEHOLD_INVITATIONS) +
            MIN_HOUSEHOLD_INVITATIONS;

        for (var j = 0; j < householdInvitationsNumber; j++)
        {
            var user = new User();
            do
            {
                user = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));
                if (householdMembers.All(h => h.UserId != user.Id) &&
                    householdInvitations.All(h => h.UserId != user.Id)) break;
            } while (true);

            var householdInvitation = new HouseholdInvitation(household, user);

            if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLD_INVITATIONS_PERCENT))
                householdInvitation.Delete();

            householdInvitations.Add(householdInvitation);

            await dbContext.AddAsync(householdInvitation);
        }

        return householdInvitations;
    }

    private async Task<Household> GenerateHousehold()
    {
        var name = RandomGenerator.GenerateRandomAlphabetString(10);
        var owner = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));

        var household = new Household(owner, name);

        if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLDS_PERCENTAJE)) household.Delete();

        await dbContext.AddAsync(household);

        var householdOwner = new HouseholdMember(household, owner);

        await dbContext.AddAsync(householdOwner);

        return household;
    }

    private async Task<List<HouseholdMember>> GenerateHouseholdMembers(Household household)
    {
        var householdMembers = new List<HouseholdMember>();
        var householdMembersNumber =
            RandomGenerator.GenerateRandomNumber(MAX_HOUSEHOLD_MEMBERS - MIN_HOUSEHOLD_MEMBERS) +
            MIN_HOUSEHOLD_MEMBERS;

        for (var j = 0; j < householdMembersNumber; j++)
        {
            var user = new User();
            do
            {
                user = users.ElementAt(RandomGenerator.GenerateRandomNumber(users.Count));
                if (householdMembers.All(h => h.UserId != user.Id)) break;
            } while (true);

            var householdMember = new HouseholdMember(household, user);

            if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_HOUSEHOLD_MEMBERS_PERCENT))
                householdMember.Delete();

            householdMembers.Add(householdMember);

            await dbContext.AddAsync(householdMember);
        }

        return householdMembers;
    }

    private async Task GenerateHouseholdStorageSpaces(Household household, List<HouseholdMember> householdMembers)
    {
        var storageSpaces = new List<StorageSpace>();
        var storageSpacesNumber =
            RandomGenerator.GenerateRandomNumber(MAX_STORAGE_SPACES - MIN_STORAGE_SPACES) +
            MIN_STORAGE_SPACES;
        var storageConditionsList = new List<StorageConditions>
        {
            StorageConditions.AmbientTemperature,
            StorageConditions.Refrigerated,
            StorageConditions.Frozen
        };

        for (var j = 0; j < storageSpacesNumber; j++)
        {
            var storageSpaceCreator =
                householdMembers.ElementAt(RandomGenerator.GenerateRandomNumber(householdMembers.Count));

            var storageSpace = new StorageSpace(RandomGenerator.GenerateRandomAlphabetString(10),
                storageConditionsList.ElementAt(RandomGenerator.GenerateRandomNumber(storageConditionsList.Count)),
                household, storageSpaceCreator.UserId);

            if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_STORAGE_SPACES_PERCENTAJE))
                storageSpace.Delete();

            storageSpaces.Add(storageSpace);

            await dbContext.AddAsync(storageSpace);
        }
    }
}