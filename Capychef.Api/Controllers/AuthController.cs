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
    public async Task<ActionResult<ApiResponse<UserDto>>> Signup(SignupCmd cmd)
    {
        var result = await authService.Signup(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.Signup");

        if (result.Failed()) return HandleError(result.Error(), logger);

        var (user, userDetails) = result.Get();

        JwtService.CreateAndSendJwt(userDetails, Response);

        return new ApiResponse<UserDto>(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Login(LoginCmd cmd)
    {
        var result = await authService.Login(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.Login");

        if (result.Failed())
        {
            // Login shouldn't return any error data, only the INCORRECT_LOGIN_DATA code

            var error = result.Error();

            if (error is NotFoundError)
            {
                logger.LogWarning(error.Message);

                return new ObjectResult(new ApiResponse<UserDto>(new ApiError(new AppError(ErrorType.Authorization),
                        "INCORRECT_LOGIN_DATA")))
                    { StatusCode = 400 };
            }

            return HandleError(result.Error(), logger);
        }

        var (user, userDetails) = result.Get();

        JwtService.CreateAndSendJwt(userDetails, Response);

        return new ApiResponse<UserDto>(user);
    }

    [HttpGet("recover-password/{email}")]
    public async Task<ActionResult> RecoverPassword(string email)
    {
        var error = await authService.RecoverPassword(email);

        var logger = loggerFactory.CreateLogger("AuthService.RecoverPassword");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordCmd cmd)
    {
        var error = await authService.ResetPassword(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.ResetPassword");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [HttpGet("check")]
    public ActionResult CheckAuthSession()
    {
        return Ok();
    }

    [HttpGet("session")]
    public async Task<ActionResult<AuthSessionDto>> GetAuthSession()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var session = await authService.GetAuthSession(userDetails);

        var logger = loggerFactory.CreateLogger("AuthService.GetAuthSession");

        if (session.Failed()) return GlobalErrorHandler.HandleError(session.Error(), logger);

        return Ok(session.Get());
    }

    [HttpGet("guest-transference")]
    public async Task<ActionResult<string>> GuestTransference()
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var result = await authService.GuestTransference(userDetails);

        var logger = loggerFactory.CreateLogger("AuthService.GuestTransference");

        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);

        return Ok(result.Get());
    }

    [HttpPost("guest-login")]
    public async Task<ActionResult<UserDto>> GuestLogin(GuestLoginCmd cmd)
    {
        var result = await authService.GuestLogin(cmd);

        var logger = loggerFactory.CreateLogger("AuthService.GuestLogin");

        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);

        var (user, userDetails) = result.Get();

        JwtService.CreateAndSendJwt(userDetails, Response);

        return user;
    }

    private ActionResult HandleError(AppError error, ILogger logger)
    {
        if (error is UsernameInUseError)
        {
            logger.LogWarning(error.Message);

            return new ObjectResult(new ApiResponse<UserDto>(new ApiError(error, "USERNAME_IN_USE")))
                { StatusCode = 400 };
        }

        return GlobalErrorHandler.HandleError(error, logger);
    }
}