using Capychef.Common.Utils;
using Capychef.Persistence;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.Entities;

namespace Capychef.Testdata;

public class TestUsers(CapychefDbContext dbContext)
{
    private readonly int BLOCKED_USERS = 50;
    private readonly int DELETED_USERS = 50;
    private readonly int GUEST_USERS = 100;
    private readonly int NON_VERIFIED_EMAIL_USERS = 50;
    private readonly int PASSWORD_USERS = 100;

    private readonly HashSet<string> Usernames = new();
    private readonly int VERIFIED_EMAIL_USERS = 50;

    public async Task Generate()
    {
        Console.WriteLine("Generating users");
        Usernames.Clear();

        GenerateUniqueUsernames();

        await GenerateVerifiedEmailUsers();

        await GenerateNonVerifiedEmailUsers();

        await GeneratePasswordUsers();

        await GenerateGuestUsers();

        await GenerateBlockedUsers();

        await GenerateDeletedUsers();

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Generated users saved");
    }

    private async Task GenerateVerifiedEmailUsers()
    {
        Console.WriteLine("Generating verified email users");
        for (var i = 0; i < VERIFIED_EMAIL_USERS; i++)
        {
            var username = Usernames.ElementAt(0);
            Usernames.Remove(username);

            var signupCmd = new SignupCmd();
            signupCmd.Username = username;
            signupCmd.Password = username;
            signupCmd.Email = $"{username}@capychef.com";

            var user = new User(signupCmd);
            user.IsEmailValid = true;
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);

            var userTokens = UserToken.CreateEmailValidationUserToken(user);
            userTokens.IsUsed = true;
            userTokens.IsActive = false;
            userTokens.UsedAt = DateTime.UtcNow;
            dbContext.Add(userTokens);
        }

        Console.WriteLine("Done");
    }

    private async Task GenerateNonVerifiedEmailUsers()
    {
        Console.WriteLine("Generating non verified email users");
        for (var i = 0; i < NON_VERIFIED_EMAIL_USERS; i++)
        {
            var username = Usernames.ElementAt(0);
            Usernames.Remove(username);

            var signupCmd = new SignupCmd();
            signupCmd.Username = username;
            signupCmd.Password = username;
            signupCmd.Email = $"{username}@capychef.com";

            var user = new User(signupCmd);
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);

            var userTokens = UserToken.CreateEmailValidationUserToken(user);
            dbContext.Add(userTokens);
        }

        Console.WriteLine("Done");
    }

    private async Task GeneratePasswordUsers()
    {
        Console.WriteLine("Generating password users");
        for (var i = 0; i < PASSWORD_USERS; i++)
        {
            var username = Usernames.ElementAt(0);
            Usernames.Remove(username);

            var signupCmd = new SignupCmd();
            signupCmd.Username = username;
            signupCmd.Password = username;

            var user = new User(signupCmd);
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);
        }

        Console.WriteLine("Done");
    }

    private async Task GenerateGuestUsers()
    {
        Console.WriteLine("Generating guest users");
        for (var i = 0; i < GUEST_USERS; i++)
        {
            var username = Usernames.ElementAt(0);
            Usernames.Remove(username);

            var signupCmd = new SignupCmd();
            signupCmd.Username = username;

            var user = new User(signupCmd);
            dbContext.Add(user);

            var userTokens = UserToken.CreateGuestAccountTransferUserToken(user);
            dbContext.Add(userTokens);
        }

        Console.WriteLine("Done");
    }

    private async Task GenerateBlockedUsers()
    {
        Console.WriteLine("Generating blocked users");
        for (var i = 0; i < BLOCKED_USERS; i++)
        {
            var username = Usernames.ElementAt(0);
            Usernames.Remove(username);

            var signupCmd = new SignupCmd();
            signupCmd.Username = username;
            signupCmd.Password = username;

            var user = new User(signupCmd);
            user.IsBlocked = true;
            user.BlockReason = "block test";
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);
        }

        Console.WriteLine("Done");
    }

    private async Task GenerateDeletedUsers()
    {
        Console.WriteLine("Generating deleted users");
        for (var i = 0; i < DELETED_USERS; i++)
        {
            var username = Usernames.ElementAt(0);
            Usernames.Remove(username);

            var signupCmd = new SignupCmd();
            signupCmd.Username = username;
            signupCmd.Password = username;

            var user = new User(signupCmd);
            user.Delete();
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);
        }

        Console.WriteLine("Done");
    }


    private void GenerateUniqueUsernames()
    {
        var totalUsers = VERIFIED_EMAIL_USERS + NON_VERIFIED_EMAIL_USERS + PASSWORD_USERS + GUEST_USERS +
                         BLOCKED_USERS + DELETED_USERS;

        for (var i = 0; i < totalUsers; i++)
        {
            var username = "";
            do
            {
                username = RandomGenerator.GenerateRandomCapsAndNumbersString(6);
            } while (Usernames.Contains(username));

            Usernames.Add(username);
        }
    }
}