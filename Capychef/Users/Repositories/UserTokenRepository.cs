using Capychef.Persistence;
using Capychef.Users.Domain.Entities;
using Capychef.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Users.Repositories;

public class UserTokenRepository(CapychefDbContext dbContext) : IUserTokenRepository
{
    public async Task AddUserTokenAsync(UserToken token)
    {
        await dbContext.UsersTokens.AddAsync(token);
    }

    public async Task<UserToken?> GetTrackedUsableUserTokenByTokenAsync(string token)
    {
        return await dbContext.UsersTokens.Usable().Where(x => x.Token == token).FirstOrDefaultAsync();
    }
}