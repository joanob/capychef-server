using System.Security.Cryptography;

namespace Capychef.Common.Utils;

public static class RandomGenerator
{
    private const string Caps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string Alphabet = Caps + Lowercase;
    private const string CapsAndNumbers = Caps + Numbers;
    private const string AlphabetAndNumbers = Alphabet + Numbers;

    public static string GenerateRandomCapsString(int length)
    {
        return CreateRandomString(length, Caps);
    }

    public static string GenerateRandomLowercaseString(int length)
    {
        return CreateRandomString(length, Lowercase);
    }

    public static string GenerateRandomNumberString(int length)
    {
        return CreateRandomString(length, Numbers);
    }

    public static string GenerateRandomAlphabetString(int length)
    {
        return CreateRandomString(length, Alphabet);
    }

    public static string GenerateRandomCapsAndNumbersString(int length)
    {
        return CreateRandomString(length, CapsAndNumbers);
    }

    public static string GenerateRandomAlphabetAndNumbersString(int length)
    {
        return CreateRandomString(length, AlphabetAndNumbers);
    }

    private static string CreateRandomString(int length, string characterSet)
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