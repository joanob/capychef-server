using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Users.Repositories;

public class UserPasswordRepository(CapychefDbContext dbContext) : IUserPasswordRepository
{
    public async Task AddUserPasswordAsync(UserPassword password)
    {
        await dbContext.UsersPasswords.AddAsync(password);
    }
}