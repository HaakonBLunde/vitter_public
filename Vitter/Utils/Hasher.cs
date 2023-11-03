namespace Vitter.Utils;

public static class Hasher
{
    public static string CreateMd5(string input)
    {
        // Use input string to calculate MD5 hash
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}