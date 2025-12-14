namespace Capychef.Users.Domain.Interfaces;

public interface IUserService
{
    Task<bool> CheckUserByUsernameAsync(string username);
}