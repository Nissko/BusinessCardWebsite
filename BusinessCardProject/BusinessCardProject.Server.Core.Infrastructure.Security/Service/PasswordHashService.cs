using System.Security.Cryptography;
using System.Text;
using BusinessCardProject.Server.Core.Infrastructure.Security.Interface;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BusinessCardProject.Server.Core.Infrastructure.Security.Service;

public class PasswordHashService : IPasswordHash
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 310000;

    public Task<string> HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: salt,
            iterations: Iterations,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: HashSize);

        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        return Task.FromResult(Convert.ToBase64String(hashBytes));
    }

    public Task<bool> VerifyPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            return Task.FromResult(false);

        if (string.IsNullOrWhiteSpace(providedPassword))
            return Task.FromResult(false);

        try
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            if (hashBytes.Length != SaltSize + HashSize)
                return Task.FromResult(false);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] storedHash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, storedHash, 0, HashSize);

            byte[] computedHash = KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            return Task.FromResult(CryptographicOperations.FixedTimeEquals(storedHash, computedHash));
        }
        catch (FormatException)
        {
            return Task.FromResult(false);
        }
    }
}