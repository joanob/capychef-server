using Capychef.Common.Auth;
using Capychef.Users.Domain.Cmd.Auth;
using Capychef.Users.Domain.DTO;
using YourOwnBoss.Common.Result;

namespace Capychef.Users.Domain.Interfaces;

public interface IAuthService
{
    Task<Result<(UserDTO, AuthUserDetails)>> Signup(SignupCmd cmd);
    Task<Result<(UserDTO, AuthUserDetails)>> Login(LoginCmd cmd);
}