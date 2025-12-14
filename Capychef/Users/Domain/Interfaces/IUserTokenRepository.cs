using Capychef.Users.Domain.Entities;

namespace Capychef.Users.Domain.Interfaces;

public interface IUserTokenRepository
{
    Task AddUserTokenAsync(UserToken token);
}