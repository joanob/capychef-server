using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Capychef.Common.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Capychef.Api.Auth;

public static class JwtService
{
    private const string SessionIdClaim = "sessionId";
    private const string UserIdClaim = "userId";
    private const string HouseholdIdClaim = "householdId";
    private const int JwtSessionExpirationMinutes = 5;
    private const int JwtRefreshExpirationYears = 100;
    private const string JwtSessionCookieName = "CAPYCHEF_AUTH_SESSION";
    private const string JwtRefreshCookieName = "CAPYCHEF_AUTH_REFRESH";
    private const string JwtSecretName = "JWT_SECRET_KEY";

    public static void CreateAndSendJwt(AuthUserDetails authUserDetails, HttpResponse response)
    {
        var jwtSecretName = Environment.GetEnvironmentVariable(JwtSecretName);

        if (jwtSecretName == null) throw new Exception("JWT_SECRET_NAME environment variable not found");

        var jwtSecret = Encoding.UTF8.GetBytes(jwtSecretName);

        var jwtSessionExpiration = DateTime.Now.AddMinutes(JwtSessionExpirationMinutes);
        var jwtSession = CreateJwt(authUserDetails, jwtSecret, jwtSessionExpiration);
        SendJwt(jwtSession, JwtSessionCookieName, response, jwtSessionExpiration);

        var jwtRefreshExpiration = DateTime.Now.AddYears(JwtRefreshExpirationYears);
        var jwtRefresh = CreateJwt(authUserDetails, jwtSecret, jwtRefreshExpiration);
        SendJwt(jwtRefresh, JwtRefreshCookieName, response, jwtRefreshExpiration);
    }

    public static void DeleteJwt(HttpResponse response)
    {
        response.Cookies.Delete(JwtSessionCookieName);
        response.Cookies.Delete(JwtRefreshCookieName);
    }

    private static string CreateJwt(AuthUserDetails authUserDetails, byte[] jwtSecret, DateTime expiration)
    {
        var claims = new List<Claim>
        {
            new(SessionIdClaim, authUserDetails.SessionId.ToString()),
            new(UserIdClaim, authUserDetails.UserId.ToString())
        };

        if (authUserDetails.HasHouseholdId)
            claims.Add(new Claim(HouseholdIdClaim, authUserDetails.GetHouseholdId().ToString()));

        var securityKey = new SymmetricSecurityKey(jwtSecret);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.WriteToken(jwtToken);

        return jwt;
    }

    private static void SendJwt(string jwt, string cookieName, HttpResponse response, DateTime expiration)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == AppEnvironment.Dev;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !isDevelopment,
            SameSite = SameSiteMode.Strict,
            Expires = expiration
        };

        response.Cookies.Append(cookieName, jwt, cookieOptions);
    }

    public static AuthUserDetails? GetUserDetailsFromSessionJwt(HttpRequest request)
    {
        return GetJwt(request, JwtSessionCookieName);
    }

    public static AuthUserDetails? GetUserDetailsFromRefreshJwt(HttpRequest request)
    {
        return GetJwt(request, JwtRefreshCookieName);
    }

    private static AuthUserDetails? GetJwt(HttpRequest request, string cookieName)
    {
        var cookie = request.Cookies[cookieName];
        if (string.IsNullOrEmpty(cookie)) return null;

        var jwtSecretName = Environment.GetEnvironmentVariable(JwtSecretName);

        if (jwtSecretName == null) throw new Exception("JWT_SECRET_NAME environment variable not found");

        var jwtSecret = Encoding.UTF8.GetBytes(jwtSecretName);
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = handler.ValidateToken(cookie, validationParameters, out _);

            var sessionId = principal.Claims.FirstOrDefault(c => c.Type == SessionIdClaim)?.Value;
            var userId = principal.Claims.FirstOrDefault(c => c.Type == UserIdClaim)?.Value;
            if (sessionId == null || userId == null) return null;

            var householdIdClaim = principal.Claims.FirstOrDefault(c => c.Type == HouseholdIdClaim);
            if (householdIdClaim == null) return new AuthUserDetails(int.Parse(userId), int.Parse(sessionId));

            return new AuthUserDetails(int.Parse(userId), int.Parse(sessionId), int.Parse(householdIdClaim.Value));
        }
        catch (Exception)
        {
            return null;
        }
    }
}