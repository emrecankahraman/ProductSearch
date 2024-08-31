using ProductSearch.Models;

namespace ProductSearch.Repository
{
    public interface IAccountRepository
    {
        Admin GetAdminByUsernameAndPassword(string username, string password);
        void CreateAdmin(string username, string password);
        Task<List<Person>> GetAllPersonsAsync();

        void Add(Admin admin);
        void CreateUser(string username, string password);

        void SaveChanges();
    }

}
