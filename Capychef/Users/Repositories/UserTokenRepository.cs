using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Users.Repositories;

public class UserTokenRepository(CapychefDbContext dbContext) : IUserTokenRepository
{
    public async Task AddUserTokenAsync(UserToken Token)
    {
        await dbContext.UsersTokens.AddAsync(Token);
    }
}