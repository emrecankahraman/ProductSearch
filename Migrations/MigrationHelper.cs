using ProductSearch.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace ProductSearch.Migrations
{
    public class MigrationHelper
    {
        private readonly Database1Context _context;

        public MigrationHelper(Database1Context context)
        {
            _context = context;
        }

        public void UpdateExistingRecords()
        {
            var admins = _context.Admins.ToList();

            foreach (var admin in admins)
            {
                if (string.IsNullOrEmpty(admin.Role))
                {
                    admin.Role = "Admin"; // Varsayılan rolü atayın
                }

                if (admin.Salt == null || admin.Salt.Length == 0)
                {
                    byte[] salt = new byte[128 / 8];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }
                    admin.Salt = salt;
                    admin.Password = HashPassword(admin.Password, salt);
                }
            }

            _context.SaveChanges();
        }

        private string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
