

using System.Security.Cryptography;

namespace Sytycc_Service.Domain;




public class PasswordProviderService:IPasswordProvider
{
    public (string passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
    {
        if (password == null) throw new ArgumentNullException(nameof(password));

        using var hmac = new HMACSHA512();

        var passwordSalt = hmac.Key;
        var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        return (Convert.ToBase64String(passwordHash), passwordSalt);
    }

    public bool VerifyPasswordHash(string password, string storedHash, byte[] storedSalt)
    {
        if (password == null) throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrWhiteSpace(storedHash)) throw new ArgumentNullException(nameof(storedHash));
        if (storedSalt.Length != 128) throw new ArgumentException("Invalid salt length", nameof(storedSalt));

        using var hmac = new HMACSHA512(storedSalt);

        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        var computedHashString = Convert.ToBase64String(computedHash);

        return computedHashString == storedHash;
    }
}