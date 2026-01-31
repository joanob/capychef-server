using Capychef.Api.Auth;
using Capychef.Api.Errors;
using Capychef.Common.Errors;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Errors;
using Capychef.Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService, ILoggerFactory loggerFactory) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<ApiResponse<UserDTO>>> Signup(SignupCmd cmd)
    {
        var result = await authService.Signup(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.Signup");

        if (result.failed()) return handleError(result.error(), logger);

        var (user, userDetails) = result.get();

        JWTService.CreateAndSendJWT(userDetails, Response);

        return new ApiResponse<UserDTO>(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginCmd cmd)
    {
        var result = await authService.Login(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.Login");

        if (result.failed()) return GlobalErrorHandler.handleError(result.error(), logger);

        var (user, userDetails) = result.get();

        JWTService.CreateAndSendJWT(userDetails, Response);

        return user;
    }

    [HttpGet("recover-password/{email}")]
    public async Task<ActionResult> RecoverPassword(string email)
    {
        var error = await authService.RecoverPassword(email);

        var logger = loggerFactory.CreateLogger("AuthService.RecoverPassword");

        if (error != null) return GlobalErrorHandler.handleError(error, logger);

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordCmd cmd)
    {
        var error = await authService.ResetPassword(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.ResetPassword");

        if (error != null) return GlobalErrorHandler.handleError(error, logger);

        return Ok();
    }

    [HttpGet("check")]
    public async Task<ActionResult> CheckAuthSession()
    {
        return Ok();
    }

    [HttpGet("session")]
    public async Task<ActionResult<AuthSessionDTO>> GetAuthSession()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var session = await authService.GetAuthSession(userDetails);

        return Ok(session);
    }

    [HttpGet("guest-transference")]
    public async Task<ActionResult<string>> GuestTransference()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var result = await authService.GuestTransference(userDetails);

        var logger = loggerFactory.CreateLogger("AuthService.GuestTransference");

        if (result.failed()) return GlobalErrorHandler.handleError(result.error(), logger);

        return Ok(result.get());
    }

    [HttpPost("guest-login")]
    public async Task<ActionResult<UserDTO>> GuestLogin(GuestLoginCmd cmd)
    {
        var result = await authService.GuestLogin(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.GuestLogin");

        if (result.failed()) return GlobalErrorHandler.handleError(result.error(), logger);

        var (user, userDetails) = result.get();

        JWTService.CreateAndSendJWT(userDetails, Response);

        return user;
    }

    private ActionResult handleError(AppError error, ILogger logger)
    {
        if (error is UsernameInUseError)
            return new ObjectResult(new ApiResponse<UserDTO>(new ApiError(error, "USERNAME_IN_USE")))
                { StatusCode = 400 };

        return GlobalErrorHandler.handleError(error, logger);
    }
}