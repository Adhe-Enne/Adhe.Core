using System.Security.Cryptography;
using Core.Contracts.Model;
using Core.SharedServices.Security.Interfaces;

namespace Core.SharedServices.Security
{
    public class PasswordHasherService<TUser> : IPasswordHasherService<TUser> where TUser : BaseUser
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 64; // 512 bits
        private const int Iterations = 100_000;

        public void HashPassword(TUser user, string password)
        {
            using var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA512);
            user.PasswordSalt = deriveBytes.Salt;
            user.PasswordHash = deriveBytes.GetBytes(HashSize);
        }

        public bool VerifyPassword(TUser user, string password)
        {
            if (user.PasswordSalt == null || user.PasswordHash == null) return false;

            using var deriveBytes = new Rfc2898DeriveBytes(password, user.PasswordSalt, Iterations, HashAlgorithmName.SHA512);
            var computedHash = deriveBytes.GetBytes(HashSize);

            return computedHash.SequenceEqual(user.PasswordHash);
        }
    }
}