namespace Capychef.Testdata;

public class Testdata(TestUsers testUsers, TestHouseholds TestHouseholds)
{
    public async Task Generate()
    {
        Console.WriteLine("Generating test data...");
        await testUsers.Generate();
        await TestHouseholds.Generate();
        Console.WriteLine("Test data generated successfully.");
    }
}