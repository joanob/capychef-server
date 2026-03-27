using Capychef.Common.Utils;
using Capychef.Persistence;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.Entities;

namespace Capychef.Testdata;

public class TestUsers(CapychefDbContext dbContext)
{
    private readonly int _blockedUsers = 50;
    private readonly int _deletedUsers = 50;
    private readonly int _guestUsers = 100;
    private readonly int _nonVerifiedEmailUsers = 50;
    private readonly int _passwordUsers = 100;

    private readonly HashSet<string> _usernames = new();
    private readonly int _verifiedEmailUsers = 50;

    public async Task Generate()
    {
        Console.WriteLine("Generating users");
        _usernames.Clear();

        GenerateUniqueUsernames();

        GenerateVerifiedEmailUsers();

        GenerateNonVerifiedEmailUsers();

        GeneratePasswordUsers();

        GenerateGuestUsers();

        GenerateBlockedUsers();

        GenerateDeletedUsers();

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Generated users saved");
    }

    private void GenerateVerifiedEmailUsers()
    {
        Console.WriteLine("Generating verified email users");
        for (var i = 0; i < _verifiedEmailUsers; i++)
        {
            var username = _usernames.ElementAt(0);
            _usernames.Remove(username);

            var signupCmd = new SignupCmd
                { Username = username, Password = username, Email = $"{username}@capychef.com" };

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

    private void GenerateNonVerifiedEmailUsers()
    {
        Console.WriteLine("Generating non verified email users");
        for (var i = 0; i < _nonVerifiedEmailUsers; i++)
        {
            var username = _usernames.ElementAt(0);
            _usernames.Remove(username);

            var signupCmd = new SignupCmd
                { Username = username, Password = username, Email = $"{username}@capychef.com" };

            var user = new User(signupCmd);
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);

            var userTokens = UserToken.CreateEmailValidationUserToken(user);
            dbContext.Add(userTokens);
        }

        Console.WriteLine("Done");
    }

    private void GeneratePasswordUsers()
    {
        Console.WriteLine("Generating password users");
        for (var i = 0; i < _passwordUsers; i++)
        {
            var username = _usernames.ElementAt(0);
            _usernames.Remove(username);

            var signupCmd = new SignupCmd { Username = username, Password = username };

            var user = new User(signupCmd);
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);
        }

        Console.WriteLine("Done");
    }

    private void GenerateGuestUsers()
    {
        Console.WriteLine("Generating guest users");
        for (var i = 0; i < _guestUsers; i++)
        {
            var username = _usernames.ElementAt(0);
            _usernames.Remove(username);

            var signupCmd = new SignupCmd { Username = username };

            var user = new User(signupCmd);
            dbContext.Add(user);

            var userTokens = UserToken.CreateGuestAccountTransferUserToken(user);
            dbContext.Add(userTokens);
        }

        Console.WriteLine("Done");
    }

    private void GenerateBlockedUsers()
    {
        Console.WriteLine("Generating blocked users");
        for (var i = 0; i < _blockedUsers; i++)
        {
            var username = _usernames.ElementAt(0);
            _usernames.Remove(username);

            var signupCmd = new SignupCmd { Username = username, Password = username };

            var user = new User(signupCmd);
            user.IsBlocked = true;
            user.BlockReason = "block test";
            dbContext.Add(user);

            var userPassword = new UserPassword(user, username);
            dbContext.Add(userPassword);
        }

        Console.WriteLine("Done");
    }

    private void GenerateDeletedUsers()
    {
        Console.WriteLine("Generating deleted users");
        for (var i = 0; i < _deletedUsers; i++)
        {
            var username = _usernames.ElementAt(0);
            _usernames.Remove(username);

            var signupCmd = new SignupCmd { Username = username, Password = username };

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
        var totalUsers = _verifiedEmailUsers + _nonVerifiedEmailUsers + _passwordUsers + _guestUsers +
                         _blockedUsers + _deletedUsers;

        for (var i = 0; i < totalUsers; i++)
        {
            string username;
            do
            {
                username = RandomGenerator.GenerateRandomCapsAndNumbersString(6);
            } while (_usernames.Contains(username));

            _usernames.Add(username);
        }
    }
}