using ProductSearch.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace ProductSearch.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Database1Context _context;

        public AccountRepository(Database1Context context)
        {
            _context = context;
        }
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var people = await _context.People.ToListAsync();

            if (people == null)
            {
                return new List<Person>();
            }
            return people;
        }
        public Admin GetAdminByUsernameAndPassword(string username, string password)
        {
            var user = _context.Admins.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return null;
            }

            Console.WriteLine($"Stored Password: {user.Password}");
            var hashedPassword = HashPassword(password, user.Salt);
            Console.WriteLine($"Hashed Password: {hashedPassword}");

            return hashedPassword == user.Password ? user : null;
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

        public void Add(Admin admin)
        {
            _context.Admins.Add(admin);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void CreateAdmin(string username, string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = HashPassword(password, salt);
            var admin = new Admin
            {
                Username = username,
                Password = hashedPassword,
                Salt = salt,
                Role = "Admin"
            };
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }
        public void CreateUser(string username, string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPassword = HashPassword(password, salt);
            var user = new Admin
            {
                Username = username,
                Password = hashedPassword,
                Salt = salt,
                Role = "User"
            };
            _context.Admins.Add(user);
            _context.SaveChanges();
        }
    }
}

