using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
namespace mstecs_back.Helpers
{
    public static class Settings
    {
        public static void CreatePasswordHash(string password, out byte[] password_hash, out byte[] password_salt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("constraseña");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("constraseña");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                password_salt = hmac.Key;
                password_hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("constraseña");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("constraseña");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("contraseña Hash");
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("contraseña Salt");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public static string MapPath(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
    }
}
