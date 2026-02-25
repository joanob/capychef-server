using System.Net;
using System.Net.Http.Json;
using Capychef.E2E.Helpers;

namespace Capychef.E2E.Authentication;

[TestFixture]
[NonParallelizable]
public class ResetPassword
{
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
    }

    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5000") };

    [Test]
    public async Task RunAllResetPasswordCasesSequentially()
    {
        string Unique()
        {
            return Guid.NewGuid().ToString("N");
        }

        // Helper to create a user (returns username,email,password)
        async Task<(string username, string email, string password)> CreateUserAsync(string? username = null,
            string? email = null, string? password = null)
        {
            var u = username ?? "user_" + Unique();
            var e = email ?? u + "@capychef.com";
            var p = password ?? "P!" + Unique();

            var payload = new { username = u, email = e, password = p };
            var r = await _client.PostAsJsonAsync("/auth/signup", payload);
            if (!(r.StatusCode == HttpStatusCode.OK || r.StatusCode == HttpStatusCode.Created))
                Assert.Fail("Setup: failed to create user " + u + " (status " + ((int)r.StatusCode) + ")");

            return (u, e, p);
        }

        // Helper to request a password recovery token and retrieve it from DB
        async Task<string> RequestAndGetToken(string username, string email)
        {
            // call recover endpoint (assume caller has marked is_email_valid when needed)
            var r = await _client.GetAsync($"/auth/recover-password/{Uri.EscapeDataString(email)}");
            Assert.That(r.StatusCode == HttpStatusCode.OK, Is.True, "Request password reset should succeed in setup");

            // fetch latest active, unused password recovery token for this user (token_type = 1)
            var sql =
                @"SELECT token FROM users_tokens WHERE user_id = (SELECT id FROM users WHERE username = @User) AND token_type = 1 AND is_active = TRUE AND is_used = FALSE ORDER BY created_at DESC LIMIT 1";
            // Poll a few times in case token creation is slightly delayed
            string? token = null;
            const int maxAttempts = 10;
            for (var i = 0; i < maxAttempts; i++)
            {
                token = await DbHelper.QueryFirstOrDefaultAsync<string>(sql, new { User = username });
                if (!string.IsNullOrEmpty(token)) break;
                await Task.Delay(200);
            }

            Assert.IsNotNull(token, "Setup: failed to retrieve password recovery token from DB");
            return token!;
        }

        // 1) Reset password with valid token and new password
        {
            var (u1, e1, p1) = await CreateUserAsync();
            // Mark email as valid for this user immediately after creation
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
                new { User = u1 });
            var token1 = await RequestAndGetToken(u1, e1);
            var newPass1 = "New!" + Unique();
            var payload1 = new { passwordRecoveryToken = token1, password = newPass1 };
            var resp1 = await _client.PostAsJsonAsync("/auth/reset-password", payload1);
            Assert.That(resp1.StatusCode == HttpStatusCode.OK, Is.True,
                "Reset password with valid token and new password should revoke user sessions");
            var sessionsSql =
                @"SELECT COUNT(*) FROM users_sessions WHERE user_id = (SELECT id FROM users WHERE username = @User) AND is_revoked = FALSE";
            var activeSessionsCount = await DbHelper.QueryFirstOrDefaultAsync<int>(sessionsSql, new { User = u1 });
            Assert.That(activeSessionsCount, Is.EqualTo(0),
                "Reset password with valid token and new password should revoke user sessions");

            // verify login with new password works (and will create a fresh session)
            var loginPayload = new { username = u1, password = newPass1 };
            var loginResp = await _client.PostAsJsonAsync("/auth/login", loginPayload);
            Assert.That(loginResp.StatusCode == HttpStatusCode.OK, Is.True,
                "Reset password with valid token and new password");
        }

        // 2) Reset password with invalid token should fail
        {
            var (u2, e2, p2) = await CreateUserAsync();
            // Mark email as valid and ensure a token exists but we'll use an invalid one
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
                new { User = u2 });
            await RequestAndGetToken(u2, e2);
            var payload2 = new { passwordRecoveryToken = "invalidtoken", password = "Whatever1!" + Unique() };
            var resp2 = await _client.PostAsJsonAsync("/auth/reset-password", payload2);
            Assert.That(resp2.StatusCode == HttpStatusCode.BadRequest || resp2.StatusCode == HttpStatusCode.NotFound,
                Is.True, "Reset password with invalid token should fail");
        }

        // 3) Reset password with expired token should fail
        {
            var (u3, e3, p3) = await CreateUserAsync();
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
                new { User = u3 });
            var token3 = await RequestAndGetToken(u3, e3);
            // expire the token
            await DbHelper.ExecuteSqlAsync(
                "UPDATE users_tokens SET expires_at = now() - interval '1 hour' WHERE token = @Token",
                new { Token = token3 });
            var payload3 = new { passwordRecoveryToken = token3, password = "NewP!" + Unique() };
            var resp3 = await _client.PostAsJsonAsync("/auth/reset-password", payload3);
            Assert.That(resp3.StatusCode == HttpStatusCode.BadRequest || resp3.StatusCode == HttpStatusCode.NotFound,
                Is.True, "Reset password with expired token should fail");
        }

        // 4) Reset password with blocked user should fail
        {
            var (u4, e4, p4) = await CreateUserAsync();
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
                new { User = u4 });
            var token4 = await RequestAndGetToken(u4, e4);
            // block user before attempting reset
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_blocked = TRUE WHERE username = @User",
                new { User = u4 });
            var payload4 = new { passwordRecoveryToken = token4, password = "NewP!" + Unique() };
            var resp4 = await _client.PostAsJsonAsync("/auth/reset-password", payload4);
            Assert.That(resp4.StatusCode == HttpStatusCode.NotFound, Is.True,
                "Reset password with blocked user should fail");
        }

        // 5) Reset password with empty token should fail
        {
            var payload5 = new { passwordRecoveryToken = "", password = "NewP!" + Unique() };
            var resp5 = await _client.PostAsJsonAsync("/auth/reset-password", payload5);
            Assert.That(resp5.StatusCode == HttpStatusCode.BadRequest, Is.True,
                "Reset password with empty token should fail");
        }

        // 6) Reset password with valid token and empty password should fail
        {
            var (u6, e6, p6) = await CreateUserAsync();
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
                new { User = u6 });
            var token6 = await RequestAndGetToken(u6, e6);
            var payload6 = new { passwordRecoveryToken = token6, password = "" };
            var resp6 = await _client.PostAsJsonAsync("/auth/reset-password", payload6);
            Assert.That(resp6.StatusCode == HttpStatusCode.BadRequest, Is.True,
                "Reset password with valid token and empty password should fail");
        }

        // 7) Reset password with valid token but blocked user should fail
        {
            var (u7, e7, p7) = await CreateUserAsync();
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
                new { User = u7 });
            var token7 = await RequestAndGetToken(u7, e7);
            // block user after token creation
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_blocked = TRUE WHERE username = @User",
                new { User = u7 });
            var payload7 = new { passwordRecoveryToken = token7, password = "NewP!" + Unique() };
            var resp7 = await _client.PostAsJsonAsync("/auth/reset-password", payload7);
            Assert.That(resp7.StatusCode == HttpStatusCode.NotFound, Is.True,
                "Reset password with valid token but blocked user should fail");
        }

        // 8) Reset password with valid token but deleted user should fail
        {
            var (u8, e8, p8) = await CreateUserAsync();
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
                new { User = u8 });
            var token8 = await RequestAndGetToken(u8, e8);
            // soft-delete user
            await DbHelper.ExecuteSqlAsync("UPDATE users SET is_deleted = TRUE WHERE username = @User",
                new { User = u8 });
            var payload8 = new { passwordRecoveryToken = token8, password = "NewP!" + Unique() };
            var resp8 = await _client.PostAsJsonAsync("/auth/reset-password", payload8);
            Assert.That(resp8.StatusCode == HttpStatusCode.NotFound, Is.True,
                "Reset password with valid token but deleted user should fail");
        }
    }
}