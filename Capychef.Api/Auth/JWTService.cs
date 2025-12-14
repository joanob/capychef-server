using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Capychef.Common.Auth;
using Microsoft.IdentityModel.Tokens;

namespace YourOwnBoss.Common.Auth;

public class JWTService
{
    private const string SESSION_ID_CLAIM = "sessionId";
    private const string USER_ID_CLAIM = "userId";
    private const int JWT_SESSION_EXPIRATION_MINUTES = 5;
    private const int JWT_REFRESH_EXPIRATION_YEARS = 100;
    private const string JWT_SESSION_COOKIE_NAME = "CAPYCHEF_AUTH_SESSION";
    private const string JWT_REFRESH_COOKIE_NAME = "CAPYCHEF_AUTH_REFRESH";

    public static void CreateAndSendJWT(AuthUserDetails authUserDetails, HttpResponse response)
    {
        var jwtSecret = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY"));

        var jwtSessionExpiration = DateTime.Now.AddMinutes(JWT_SESSION_EXPIRATION_MINUTES);
        var jwtSession = createJWT(authUserDetails, jwtSecret, jwtSessionExpiration);
        sendJWT(jwtSession, JWT_SESSION_COOKIE_NAME, response, jwtSessionExpiration);

        var jwtRefreshExpiration = DateTime.Now.AddYears(JWT_REFRESH_EXPIRATION_YEARS);
        var jwtRefresh = createJWT(authUserDetails, jwtSecret, jwtRefreshExpiration);
        sendJWT(jwtRefresh, JWT_REFRESH_COOKIE_NAME, response, jwtRefreshExpiration);
    }

    public static void DeleteJWT(HttpResponse response)
    {
        response.Cookies.Delete(JWT_SESSION_COOKIE_NAME);
        response.Cookies.Delete(JWT_REFRESH_COOKIE_NAME);
    }

    private static string createJWT(AuthUserDetails authUserDetails, byte[] jwtSecret, DateTime expiration)
    {
        var claims = new List<Claim>
        {
            new(SESSION_ID_CLAIM, authUserDetails.SessionId.ToString()),
            new(USER_ID_CLAIM, authUserDetails.UserId.ToString())
        };

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

    private static void sendJWT(string jwt, string cookieName, HttpResponse response, DateTime expiration)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = expiration
        };

        response.Cookies.Append(cookieName, jwt, cookieOptions);
    }

    public static AuthUserDetails getUserDetailsFromSessionJWT(HttpRequest request)
    {
        return getJWT(request, JWT_SESSION_COOKIE_NAME);
    }

    public static AuthUserDetails getUserDetailsFromRefreshJWT(HttpRequest request)
    {
        return getJWT(request, JWT_REFRESH_COOKIE_NAME);
    }

    private static AuthUserDetails getJWT(HttpRequest request, string cookieName)
    {
        var cookie = request.Cookies[cookieName];
        if (string.IsNullOrEmpty(cookie)) return null;

        var jwtSecret = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));
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

            var sessionId = principal.Claims.FirstOrDefault(c => c.Type == SESSION_ID_CLAIM)?.Value;
            var userId = principal.Claims.FirstOrDefault(c => c.Type == USER_ID_CLAIM)?.Value;
            if (sessionId == null || userId == null) return null;

            return new AuthUserDetails(int.Parse(userId), int.Parse(sessionId));
        }
        catch (Exception e)
        {
            return null;
        }
    }
}