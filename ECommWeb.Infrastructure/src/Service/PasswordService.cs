using System.Security.Cryptography;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace ECommWeb.Infrastructure.src;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(16);
        var hashedPassword = GetHashedPassword(password, salt);
        return hashedPassword;
    }

    public bool VerifyPassword(string password, string passwordHash, byte[] salt)
    {
        var hashedPassword = GetHashedPassword(password, salt);
        return hashedPassword == passwordHash;
    }

    private string GetHashedPassword(string password, byte[] salt)
    {
        string hashedPassword = Convert.ToBase64String(
            KeyDerivation.Pbkdf2
            (
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            )
        );
        return hashedPassword;
    }
}
