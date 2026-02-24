using System.Net;
using System.Net.Http.Json;

namespace Capychef.E2E.Authentication;

[TestFixture]
[NonParallelizable]
public class Signup
{
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
    }

    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5000") };

    [Test]
    public async Task RunAllSignupCasesSequentially()
    {
        // All strings will be Guid to avoid collisions with existing data
        string Unique()
        {
            return Guid.NewGuid().ToString("N");
        }

        // 1) Create a new user with username, email and password 
        var username1 = Unique();
        var email1 = username1 + "@capychef.com";
        var password1 = Unique();
        var payload1 = new { username = username1, email = email1, password = password1 };
        var resp1 = await _client.PostAsJsonAsync("/auth/signup", payload1);
        Assert.That(resp1.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Create a new user with username, email and password");

        // 2) Create a new user with username and password 
        var username2 = Unique();
        var password2 = Unique();
        var payload2 = new { username = username2, password = password2 };
        var resp2 = await _client.PostAsJsonAsync("/auth/signup", payload2);
        Assert.That(resp2.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Create a new user with username and password");

        // 3) Create a guest user
        var username3 = Unique();
        var payload3 = new { username = username3 };
        var resp3 = await _client.PostAsJsonAsync("/auth/signup", payload3);
        Assert.That(resp3.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Create a guest user");

        // 4) Create user with empty username should fail
        var payload4 = new { username = "" };
        var resp4 = await _client.PostAsJsonAsync("/auth/signup", payload4);
        Assert.That(resp4.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
            "Create user with empty username should fail");

        // 5) Create a user with an existing username should fail
        var payload5 = new { username = username1 };
        var resp5 = await _client.PostAsJsonAsync("/auth/signup", payload5);
        Assert.That(resp5.StatusCode == HttpStatusCode.BadRequest, Is.True,
            "Create a user with an existing username should fail");

        // 6) Create user with invalid email should fail
        var payload6 = new { email = "not-an-email", password = Unique() };
        var resp6 = await _client.PostAsJsonAsync("/auth/signup", payload6);
        Assert.That(resp6.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
            "Create user with invalid email should fail");

        // 7) Create a user with an existing email should fail
        var payload7 = new { username = Unique(), email = email1, password = Unique() };
        var resp7 = await _client.PostAsJsonAsync("/auth/signup", payload7);
        Assert.That(resp7.StatusCode == HttpStatusCode.BadRequest, Is.True,
            "Create a user with an existing email should fail");
    }
}