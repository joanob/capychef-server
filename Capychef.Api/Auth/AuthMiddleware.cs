using Capychef.Common.Auth;
using Capychef.Users.Domain.Interfaces;

namespace Capychef.Api.Auth;

public class AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
{
    private readonly string[] _publicRoutes =
    {
        "/data/load",
        "/testdata",
        "/auth/signup",
        "/auth/login",
        "/users/username/check",
        "/users/email/validate",
        "/auth/recover-password",
        "/auth/reset-password",
        "/auth/guest-login"
    };

    public async Task InvokeAsync(HttpContext context)
    {
        if (_publicRoutes.Any(x => context.Request.Path.Value?.StartsWith(x) ?? false))
        {
            await next(context);
            return;
        }

        var userDetails = JwtService.GetUserDetailsFromSessionJwt(context.Request);
        if (userDetails != null)
        {
            LogAuthUserDetails(userDetails);
            AuthUserDetailsService.AddAuthUserDetailsToContext(context, userDetails);
            await next(context);
            return;
        }

        userDetails = JwtService.GetUserDetailsFromRefreshJwt(context.Request);
        if (userDetails != null)
        {
            var userSesionService = context.RequestServices.GetRequiredService<IUserSessionService>();
            if (await userSesionService.ValidateAuthUserSession(userDetails))
            {
                LogAuthUserDetails(userDetails);
                JwtService.CreateAndSendJwt(userDetails, context.Response);
                AuthUserDetailsService.AddAuthUserDetailsToContext(context, userDetails);
                await next(context);
                return;
            }

            logger.LogWarning($"invalid session {userDetails.SessionId} for user {userDetails.UserId}");
        }

        context.Response.StatusCode = 401;
    }

    private void LogAuthUserDetails(AuthUserDetails userDetails)
    {
        if (userDetails.HasHouseholdId)
            logger.LogInformation(
                $"user {userDetails.UserId} - session {userDetails.SessionId} - household {userDetails.GetHouseholdId()}");
        else
            logger.LogInformation($"user {userDetails.UserId} - session {userDetails.SessionId}");
    }
}