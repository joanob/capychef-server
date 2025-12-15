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

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginCmd cmd)
    {
        var result = await authService.Login(cmd);

        if (result.failed()) return GlobalErrorHandler.handleError(result.error());

        var (user, userDetails) = result.get();

        JWTService.CreateAndSendJWT(userDetails, Response);

        return user;
    }

    [HttpGet("recover-password/{email}")]
    public async Task<ActionResult> RecoverPassword(string email)
    {
        var error = await authService.RecoverPassword(email);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordCmd cmd)
    {
        var error = await authService.ResetPassword(cmd);

        if (error != null) return GlobalErrorHandler.handleError(error);

        return Ok();
    }

    [HttpGet("check")]
    public async Task<ActionResult> CheckAuthSession()
    {
        return Ok();
    }

    [HttpGet("guest-transference")]
    public async Task<ActionResult<string>> GuestTransference()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var result = await authService.GuestTransference(userDetails);

        if (result.failed()) return GlobalErrorHandler.handleError(result.error());

        return Ok(result.get());
    }

    [HttpPost("guest-login")]
    public async Task<ActionResult<UserDTO>> GuestLogin(GuestLoginCmd cmd)
    {
        var result = await authService.GuestLogin(cmd);

        if (result.failed()) return GlobalErrorHandler.handleError(result.error());

        var (user, userDetails) = result.get();

        JWTService.CreateAndSendJWT(userDetails, Response);

        return user;
    }
}