using System.Security.Cryptography;
using System.Text;

namespace PBKDF2Demo;

public class PBKDF2Service
{
    public static void Test()
    {
        var password = "P@ssw0rd"; // Password
        var salt = Encoding.ASCII.GetBytes("this_is_salt"); // Salt
        
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1500, HashAlgorithmName.SHA256); // PBKDF2
        Console.WriteLine($"PBKDF2: {BitConverter.ToString(pbkdf2.GetBytes(15))}");
        Console.WriteLine($"HashAlgorithm: {pbkdf2.HashAlgorithm}");
        Console.WriteLine($"IterationCount: {pbkdf2.IterationCount}");
    }
}