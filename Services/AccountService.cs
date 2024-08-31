using ProductSearch.Repository;
using System.Collections.Generic;
using System.Security.Claims;

namespace ProductSearch.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _accountRepository.GetAdminByUsernameAndPassword(username, password);
            return user != null;
        }
        public int GetCurrentUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new Exception("User ID not found or invalid.");
        }

        public ClaimsPrincipal Login(string username, string password) 
        {
            var user = _accountRepository.GetAdminByUsernameAndPassword(username, password);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }
            if (string.IsNullOrEmpty(user.Role) || user.Salt == null || user.Salt.Length == 0)
            {
                throw new Exception("User data is incomplete. Please update the user record.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.AdminId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, "Cookie");
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
       
    }
}
