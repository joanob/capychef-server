using System.Security.Cryptography;

namespace Capychef.Common.Utils;

public static class RandomGenerator
{
    private static readonly string caps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string capsAndNumbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private static readonly string
        alphabetAndNumbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomCapsString(int length)
    {
        return createRandomString(length, caps);
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
}