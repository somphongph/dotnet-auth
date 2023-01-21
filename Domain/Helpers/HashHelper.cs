using System.Security.Cryptography;
using System.Text;

namespace Domain.Helpers;

public class HashHelper
{
    public static string GenerateHash(string pass, string salt)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(pass + salt);
        bytes = SHA256.Create().ComputeHash(bytes);

        return Convert.ToBase64String(bytes);
    }

    public static string GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();
        var buff = new byte[24];
        rng.GetBytes(buff);

        return Convert.ToBase64String(buff);
    }
}
