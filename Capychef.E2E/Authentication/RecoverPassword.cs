using System.Net;
using System.Net.Http.Json;
using Capychef.E2E.Helpers;

namespace Capychef.E2E.Authentication;

[TestFixture]
[NonParallelizable]
public class RecoverPassword
{
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
    }

    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5000") };

    [Test]
    public async Task RunAllRecoverPasswordCasesSequentially()
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

        // 1) Request password reset with valid email
        var (u1, email1, _) = await CreateUserAsync();
        // Mark email as valid for this user
        await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
            new { User = u1 });
        var resp1 = await _client.GetAsync($"/auth/recover-password/{Uri.EscapeDataString(email1)}");
        Assert.That(resp1.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Request password reset with valid email");

        // 2) Request password reset with invalid email should fail
        var (_, email2, _) = await CreateUserAsync();
        var resp2 = await _client.GetAsync($"/auth/recover-password/{Uri.EscapeDataString(email2)}");
        Assert.That(resp2.StatusCode == HttpStatusCode.NotFound, Is.True,
            "Request password reset with invalid email should fail");

        // 3) Request password reset with non-existent email should fail
        var nonExistent = "noone_" + Unique() + "@example.com";
        var resp3 = await _client.GetAsync($"/auth/recover-password/{Uri.EscapeDataString(nonExistent)}");
        Assert.That(resp3.StatusCode == HttpStatusCode.NotFound, Is.True,
            "Request password reset with non-existent email should fail");

        // 4) Request password reset with blocked user should fail
        var (u4, e4, _) = await CreateUserAsync();
        // Ensure email is valid, then mark user as blocked
        await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
            new { User = u4 });
        var updateBlockedSql = "UPDATE users SET is_blocked = TRUE WHERE username = @User";
        var updated = await DbHelper.ExecuteSqlAsync(updateBlockedSql, new { User = u4 });
        Assert.That(updated, Is.GreaterThan(0), "Setup: failed to mark user as blocked in DB");
        var resp4 = await _client.GetAsync($"/auth/recover-password/{Uri.EscapeDataString(e4)}");
        Assert.That(resp4.StatusCode == HttpStatusCode.NotFound, Is.True,
            "Request password reset with blocked user should fail");

        // 5) Request password reset with deleted user should fail
        var (u5, e5, _) = await CreateUserAsync();
        // Ensure email is valid, then mark user as deleted
        await DbHelper.ExecuteSqlAsync("UPDATE users SET is_email_valid = TRUE WHERE username = @User",
            new { User = u5 });
        var updateDeletedSql = "UPDATE users SET is_deleted = TRUE WHERE username = @User";
        var updatedDel = await DbHelper.ExecuteSqlAsync(updateDeletedSql, new { User = u5 });
        Assert.That(updatedDel, Is.GreaterThan(0), "Setup: failed to mark user as deleted in DB");
        var resp5 = await _client.GetAsync($"/auth/recover-password/{Uri.EscapeDataString(e5)}");
        Assert.That(resp5.StatusCode == HttpStatusCode.NotFound, Is.True,
            "Request password reset with deleted user should fail");
    }
}