using System.Security.Claims;

namespace ProductSearch.Services
{
    public interface IAccountService
    {
        bool ValidateUser(string username, string password);
        ClaimsPrincipal Login(string username, string password);
        int GetCurrentUserId(ClaimsPrincipal user);

    }
}
