namespace Capychef.Api.Auth;

public static class RateLimiterPolicies
{
    public const string Signup = "signup";
    public const string UsernameCheck = "username_check";
    public const string EmailValidation = "email_validation";
    public const string PasswordRecovery = "password_recovery";
}