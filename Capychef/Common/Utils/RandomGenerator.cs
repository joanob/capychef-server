using System.Security.Cryptography;

namespace Capychef.Common.Utils;

public static class RandomGenerator
{
    private static readonly string caps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string lowercase = "abcdefghijklmnopqrstuvwxyz";
    private static readonly string numbers = "0123456789";
    private static readonly string alphabet = caps + lowercase;
    private static readonly string capsAndNumbers = caps + numbers;
    private static readonly string alphabetAndNumbers = alphabet + numbers;

    public static string GenerateRandomCapsString(int length)
    {
        return createRandomString(length, caps);
    }

    public static string GenerateRandomLowercaseString(int length)
    {
        return createRandomString(length, lowercase);
    }

    public static string GenerateRandomNumberString(int length)
    {
        return createRandomString(length, numbers);
    }

    public static string GenerateRandomAlphabetString(int length)
    {
        return createRandomString(length, alphabet);
    }

    public static string GenerateRandomCapsAndNumbersString(int length)
    {
        return createRandomString(length, capsAndNumbers);
    }

    public static string GenerateRandomAlphabetAndNumbersString(int length)
    {
        return createRandomString(length, alphabetAndNumbers);
    }

    private static string createRandomString(int length, string characterSet)
    {
        var resut = new char[length];
        var buffer = new byte[length];

        RandomNumberGenerator.Fill(buffer);

        for (var i = 0; i < length; i++) resut[i] = characterSet[buffer[i] % characterSet.Length];

        return new string(resut);
    }

    public static bool GenerateRandomBoolPercentage(float percentage)
    {
        return RandomNumberGenerator.GetInt32(100) < percentage * 100;
    }

    public static int GenerateRandomNumber(int max)
    {
        return RandomNumberGenerator.GetInt32(max);
    }
}