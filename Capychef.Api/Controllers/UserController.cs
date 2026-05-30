using Capychef.Api.Auth;
using Capychef.Api.Errors;
using Capychef.Users.Domain.Cmd;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService, ILoggerFactory loggerFactory) : ControllerBase
{
    [HttpGet("username/check/{username}")]
    [EnableRateLimiting(RateLimiterPolicies.UsernameCheck)]
    public async Task<ActionResult<UserDto>> CheckUsername(string username)
    {
        var result = await userService.CheckUserByUsernameAsync(username);

        return result ? Ok() : NotFound();
    }

    [HttpGet("email/validate/{token}")]
    [EnableRateLimiting(RateLimiterPolicies.EmailValidation)]
    public async Task<ActionResult> ValidateEmail(string token)
    {
        var error = await userService.ValidateEmailAsync(token);

        var logger = loggerFactory.CreateLogger("UserService.ValidateEmailAsync");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<UserDto>>> ChangePassword(ChangePasswordCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var result = await userService.ChangePassword(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("AuthService.ChangePassword");

        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);

        var (user, newUserDetails) = result.Get();

        JwtService.CreateAndSendJwt(newUserDetails, Response);

        return new ApiResponse<UserDto>(user);
    }

    [HttpDelete("me")]
    public async Task<ActionResult> DeleteAccount(DeleteAccountCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await userService.DeleteAccount(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("UserService.DeleteAccount");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        JwtService.DeleteJwt(Response);

        return Ok();
    }
}