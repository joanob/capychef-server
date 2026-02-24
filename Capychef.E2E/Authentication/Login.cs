using System.Net;
using System.Net.Http.Json;
using Capychef.E2E.Helpers;

namespace Capychef.E2E.Authentication;

[TestFixture]
[NonParallelizable]
public class Login
{
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
    }

    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5000") };

    [Test]
    public async Task RunAllLoginCasesSequentially()
    {
        string Unique()
        {
            return Guid.NewGuid().ToString("N");
        }

        // Helper to create a user (returns tuple username,email,password)
        async Task<(string username, string email, string password)> CreateUserAsync(string? username = null,
            string? email = null, string? password = null)
        {
            var u = username ?? Unique();
            var e = email ?? u + "@capychef.com";
            var p = password ?? Unique();

            var payload = new { username = u, email = e, password = p };
            var r = await _client.PostAsJsonAsync("/auth/signup", payload);
            // We accept Created or OK depending on API behavior
            if (r.StatusCode != HttpStatusCode.OK)
                Assert.Fail($"Setup: failed to create user {u} (status {r.StatusCode})");

            return (u, e, p);
        }

        // 1) Login with valid username and password
        var (u1, _, p1) = await CreateUserAsync();
        var payload1 = new { username = u1, password = p1 };
        var resp1 = await _client.PostAsJsonAsync("/auth/login", payload1);
        Assert.That(resp1.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Login with valid username and password");

        // 2) Login with valid email and password
        var (_, e2, p2) = await CreateUserAsync();
        var payload2 = new { username = e2, password = p2 };
        var resp2 = await _client.PostAsJsonAsync("/auth/login", payload2);
        Assert.That(resp2.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Login with valid email and password");

        // 3) Login with invalid username should fail
        var payload3 = new { username = "nonexistent_" + Unique(), password = "whatever" };
        var resp3 = await _client.PostAsJsonAsync("/auth/login", payload3);
        Assert.That(resp3.StatusCode == HttpStatusCode.BadRequest, Is.True, "Login with invalid username should fail");

        // 4) Login with invalid email should fail
        var payload4 = new { username = "nope@notexist.com", password = "whatever" };
        var resp4 = await _client.PostAsJsonAsync("/auth/login", payload4);
        Assert.That(resp4.StatusCode == HttpStatusCode.BadRequest, Is.True, "Login with invalid email should fail");

        // 5) Login with invalid password should fail
        var (u5, _, _) = await CreateUserAsync();
        var payload5 = new { username = u5, password = "wrongpass" };
        var resp5 = await _client.PostAsJsonAsync("/auth/login", payload5);
        Assert.That(resp5.StatusCode == HttpStatusCode.BadRequest, Is.True, "Login with invalid password should fail");

        // 6) Login to a blocked user should fail
        var (u6, _, p6) = await CreateUserAsync();
        // Mark user as blocked in DB
        var updateBlockedSql = "UPDATE users SET is_blocked = TRUE WHERE username = @User";
        var updated = await DbHelper.ExecuteSqlAsync(updateBlockedSql, new { User = u6 });
        var payload6 = new { username = u6, password = p6 };
        var resp6 = await _client.PostAsJsonAsync("/auth/login", payload6);
        Assert.That(resp6.StatusCode == HttpStatusCode.BadRequest, Is.True, "Login to a blocked user should fail");

        // 7) Login to a deleted user should fail
        var (u7, _, p7) = await CreateUserAsync();
        // Mark user as deleted in DB (soft delete)
        var updateDeletedSql = "UPDATE users SET is_deleted = TRUE WHERE username = @User";
        var updatedDel = await DbHelper.ExecuteSqlAsync(updateDeletedSql, new { User = u7 });
        var payload7 = new { username = u7, password = p7 };
        var resp7 = await _client.PostAsJsonAsync("/auth/login", payload7);
        Assert.That(resp7.StatusCode == HttpStatusCode.BadRequest, Is.True, "Login to a deleted user should fail");
    }
}