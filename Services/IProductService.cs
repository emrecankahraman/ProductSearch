using System.Linq;
using ProductSearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductSearch.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetFilteredProductsAsync(string productName, string productKeyword, string description, string supervisorName, string supervisorLastName, string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment);
         Task <IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> SearchByMultipleCriteriaAsync(string productName, string productKeyword, string description, string supervisorName, string supervisorLastName, string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment);


        Task<(bool success, List<string> updatedKeywords)> UpdateProductAsync( int id,string selectedValue,string newValue,int currentUserId);
        Task<Product> CreateProductAsync(string Name, string Keywords, string Description, string Department,
                                    string supervisorName, string supervisorLastName, string supervisorDepartment,
                                    string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment, int currentUserId, int Quantity,
                                    Product product);
        Task DeleteProductAsync(int id);
        Task<List<string>> GetKeywordsAsync(int id);

    }
}
