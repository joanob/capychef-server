namespace Capychef.Testdata;

public class Testdata(TestUsers testUsers, TestHouseholds testHouseholds, TestFood testFood)
{
    public async Task Generate()
    {
        Console.WriteLine("Generating test data...");
        await testUsers.Generate();
        await testHouseholds.Generate();
        await testFood.Generate();
        Console.WriteLine("Test data generated successfully.");
    }
}