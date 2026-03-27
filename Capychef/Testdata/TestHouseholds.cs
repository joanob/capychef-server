using Capychef.Common.Utils;
using Capychef.Households.Domain.Entities;
using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Testdata;

public class TestHouseholds(CapychefDbContext dbContext)
{
    private readonly float _deletedHouseholdInvitationsPercent = 0.2f;
    private readonly float _deletedHouseholdJoinRequestsPercent = 0.2f;
    private readonly float _deletedHouseholdMembersPercent = 0.2f;
    private readonly float _deletedHouseholdsPercentaje = 0.2f;
    private readonly float _deletedStorageSpacesPercentaje = 0.2f;
    private readonly int _households = 200;
    private readonly int _maxHouseholdInvitations = 10;
    private readonly int _maxHouseholdJoinRequests = 10;
    private readonly int _maxHouseholdMembers = 5;
    private readonly int _maxStorageSpaces = 5;
    private readonly int _minHouseholdInvitations = 0;
    private readonly int _minHouseholdJoinRequests = 0;
    private readonly int _minHouseholdMembers = 2;
    private readonly int _minStorageSpaces = 0;

    private List<User> _users = new();

    public async Task Generate()
    {
        _users = await dbContext.Users.ToListAsync();

        Console.WriteLine("Generating households");

        for (var i = 0; i < _households; i++)
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
            RandomGenerator.GenerateRandomNumber(_maxHouseholdJoinRequests - _minHouseholdJoinRequests) +
            _minHouseholdJoinRequests;

        for (var j = 0; j < householdJoinRequestsNumber; j++)
        {
            User user;
            do
            {
                user = _users.ElementAt(RandomGenerator.GenerateRandomNumber(_users.Count));
                if (householdMembers.All(h => h.UserId != user.Id) &&
                    householdInvitations.All(h => h.UserId != user.Id) &&
                    householdJoinRequests.All(h => h.UserId != user.Id)) break;
            } while (true);

            var householdJoinRequest = new HouseholdJoinRequest(household, user);

            if (RandomGenerator.GenerateRandomBoolPercentage(_deletedHouseholdJoinRequestsPercent))
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
            RandomGenerator.GenerateRandomNumber(_maxHouseholdInvitations - _minHouseholdInvitations) +
            _minHouseholdInvitations;

        for (var j = 0; j < householdInvitationsNumber; j++)
        {
            User user;
            do
            {
                user = _users.ElementAt(RandomGenerator.GenerateRandomNumber(_users.Count));
                if (householdMembers.All(h => h.UserId != user.Id) &&
                    householdInvitations.All(h => h.UserId != user.Id)) break;
            } while (true);

            var householdInvitation = new HouseholdInvitation(household, user);

            if (RandomGenerator.GenerateRandomBoolPercentage(_deletedHouseholdInvitationsPercent))
                householdInvitation.Delete();

            householdInvitations.Add(householdInvitation);

            await dbContext.AddAsync(householdInvitation);
        }

        return householdInvitations;
    }

    private async Task<Household> GenerateHousehold()
    {
        var name = RandomGenerator.GenerateRandomAlphabetString(10);
        var owner = _users.ElementAt(RandomGenerator.GenerateRandomNumber(_users.Count));

        var household = new Household(owner, name);

        if (RandomGenerator.GenerateRandomBoolPercentage(_deletedHouseholdsPercentaje)) household.Delete();

        await dbContext.AddAsync(household);

        var householdOwner = new HouseholdMember(household, owner);

        await dbContext.AddAsync(householdOwner);

        return household;
    }

    private async Task<List<HouseholdMember>> GenerateHouseholdMembers(Household household)
    {
        var householdMembers = new List<HouseholdMember>();
        var householdMembersNumber =
            RandomGenerator.GenerateRandomNumber(_maxHouseholdMembers - _minHouseholdMembers) +
            _minHouseholdMembers;

        for (var j = 0; j < householdMembersNumber; j++)
        {
            User user;
            do
            {
                user = _users.ElementAt(RandomGenerator.GenerateRandomNumber(_users.Count));
                if (householdMembers.All(h => h.UserId != user.Id)) break;
            } while (true);

            var householdMember = new HouseholdMember(household, user);

            if (RandomGenerator.GenerateRandomBoolPercentage(_deletedHouseholdMembersPercent))
                householdMember.Delete();

            householdMembers.Add(householdMember);

            await dbContext.AddAsync(householdMember);
        }

        return householdMembers;
    }

    private async Task GenerateHouseholdStorageSpaces(Household household, List<HouseholdMember> householdMembers)
    {
        var storageSpacesNumber =
            RandomGenerator.GenerateRandomNumber(_maxStorageSpaces - _minStorageSpaces) +
            _minStorageSpaces;
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

            if (RandomGenerator.GenerateRandomBoolPercentage(_deletedStorageSpacesPercentaje))
                storageSpace.Delete();

            await dbContext.AddAsync(storageSpace);
        }
    }
}