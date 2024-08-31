using ProductSearch.Models;
using ProductSearch.Repositories;
using ProductSearch.Repository;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace ProductSearch.Services
{
    public class AdminService : IAdminService
    {
        private readonly IProductRepository _productRepository;
        private readonly IAccountRepository _accountRepository;

        public AdminService(IProductRepository productRepository, IAccountRepository accountRepository)
        {
            _productRepository = productRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Product>> GetDashboardDataAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }
        
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            return await _accountRepository.GetAllPersonsAsync();
        }
        public void AddAdmin(Admin admin)
        {
            _accountRepository.CreateAdmin(admin.Username, admin.Password);
        }

       
        public void AddUser(Admin user)
        {
            _accountRepository.CreateUser(user.Username, user.Password);
        }
    }
}
