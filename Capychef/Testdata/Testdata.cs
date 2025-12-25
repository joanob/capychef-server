namespace Capychef.Testdata;

public class Testdata(TestUsers testUsers)
{
    public async Task Generate()
    {
        testUsers.Generate();
    }
}