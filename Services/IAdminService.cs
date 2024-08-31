using ProductSearch.Models;

namespace ProductSearch.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<Product>> GetDashboardDataAsync();
        void AddAdmin(Admin admin);
        void AddUser(Admin user);
        Task<List<Person>> GetAllPersonsAsync();
    }
}