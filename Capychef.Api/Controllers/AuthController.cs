using Capychef.Users.Domain.Cmd.Auth;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<UserDTO>> Signup(SignupCmd cmd)
    {
        var result = await authService.Signup(cmd);

        if (result.failed()) return GlobalErrorHandler.handleError(result.error());

        var (user, userDetails) = result.get();

        JWTService.CreateAndSendJWT(userDetails, Response);

        return user;
    }
}