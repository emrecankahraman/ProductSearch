using System;
using System.Collections.Generic;
using System.Linq;
using ProductSearch.Models;
using ProductSearch.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductSearch.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }




        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }
        public async Task<List<string>> GetKeywordsAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product?.Keywords?.Select(k => k.KeywordText).ToList() ?? new List<string>();
        }

        public async Task<IEnumerable<Product>> SearchByMultipleCriteriaAsync(string productName, string productKeyword, string description, string supervisorName, string supervisorLastName, string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment)
        {
            var products = await _productRepository.GetAllProductsAsync();
            return FilterProducts(products, productName, productKeyword, description, supervisorName, supervisorLastName, Department, coSupervisorName, coSupervisorLastName, coSupervisorDepartment);
        }

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(string productName, string productKeyword, string description, string supervisorName,
 string supervisorLastName, string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment)
        {
            var products = await _productRepository.GetAllProductsAsync();
            Console.WriteLine($"Total products fetched: {products.Count()}");

            var filteredProducts = FilterProducts(products, productName, productKeyword, description, supervisorName, supervisorLastName, Department, coSupervisorName, coSupervisorLastName, coSupervisorDepartment);
            Console.WriteLine($"Filtered products count: {filteredProducts.Count()}");

            return filteredProducts;
        }

        private IEnumerable<Product> FilterProducts(IEnumerable<Product> results, string productName, string productKeyword, string description, string supervisorName,
      string supervisorLastName, string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment)
        {
            if (results == null)
            {
                Console.WriteLine("Results is null");
                return Enumerable.Empty<Product>();
            }

            try
            {
                Console.WriteLine("Original Results:");
                foreach (var result in results)
                {
                    Console.WriteLine($"Name: {result.Name}, Description: {result.Description}");
                }

                results = results.Where(t =>
                    (string.IsNullOrEmpty(productName) || t.Name.Contains(productName)) &&
                    (string.IsNullOrEmpty(description) || t.Description.Contains(description)) &&
                    (string.IsNullOrEmpty(supervisorName) || t.Supervisor.Name.Contains(supervisorName)) &&
                    (string.IsNullOrEmpty(supervisorLastName) || t.Supervisor.Surname.Contains(supervisorLastName)) &&
                    (string.IsNullOrEmpty(Department) || t.Supervisor.Department.Contains(Department)) &&
                    (string.IsNullOrEmpty(coSupervisorName) || t.CoSupervisor.Name.Contains(coSupervisorName)) &&
                    (string.IsNullOrEmpty(coSupervisorLastName) || t.CoSupervisor.Surname.Contains(coSupervisorLastName)) &&
                    (string.IsNullOrEmpty(coSupervisorDepartment) || t.CoSupervisor.Department.Contains(coSupervisorDepartment)) &&
                    (string.IsNullOrEmpty(productKeyword) || t.Keywords.Any(k => k.KeywordText.Contains(productKeyword)))
                );

                Console.WriteLine("Filtered Results:");
                foreach (var result in results)
                {
                    Console.WriteLine($"Filtered Name: {result.Name}, Description: {result.Description}");
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Enumerable.Empty<Product>();
            }
        }






        public async Task<(bool success, List<string> updatedKeywords)> UpdateProductAsync(int id, string selectedValue, string newValue, int currentUserId)
        {

            var productToUpdate = await _productRepository.GetProductByIdAsync(id);

            if (productToUpdate == null)
            {
                return (false, null);
            }

            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(newValue))
            {
                switch (selectedValue)
                {
                    case "Name":
                        productToUpdate.Name = newValue;
                        break;
                    case "Description":
                        productToUpdate.Description = newValue;
                        break;
                    case "SupervisorName":
                        if (productToUpdate.Supervisor == null)
                        {
                            productToUpdate.Supervisor = new Supervisor();
                        }
                        productToUpdate.Supervisor.Name = newValue;
                        break;
                    case "SupervisorLastName":
                        if (productToUpdate.Supervisor == null)
                        {
                            productToUpdate.Supervisor = new Supervisor();
                        }
                        productToUpdate.Supervisor.Surname = newValue;
                        break;
                    case "SupervisorDepartment":
                        if (productToUpdate.Supervisor == null)
                        {
                            productToUpdate.Supervisor = new Supervisor();
                        }
                        productToUpdate.Supervisor.Department = newValue;
                        break;
                    case "CoSupervisorName":
                        if (productToUpdate.CoSupervisor == null)
                        {
                            productToUpdate.CoSupervisor = new Cosupervisor();
                        }
                        productToUpdate.CoSupervisor.Name = newValue;
                        break;
                    case "CoSupervisorLastName":
                        if (productToUpdate.CoSupervisor == null)
                        {
                            productToUpdate.CoSupervisor = new Cosupervisor();
                        }
                        productToUpdate.CoSupervisor.Surname = newValue;
                        break;
                    case "CoSupervisorDepartment":
                        if (productToUpdate.CoSupervisor == null)
                        {
                            productToUpdate.CoSupervisor = new Cosupervisor();
                        }
                        productToUpdate.CoSupervisor.Department = newValue;
                        break;
                    case "Quantity":
                        if (int.TryParse(newValue, out int quantity))
                        {
                            productToUpdate.Quantity = quantity;
                        }
                        else
                        {
                            return (false, null);
                        }
                        break;
                    case "Keywords":
                        UpdateKeywords(productToUpdate, newValue);
                        break;
                    default:
                        return (false, null);
                }
                productToUpdate.UpdateDate = DateTime.Now;
                productToUpdate.UpdatedById = currentUserId;

                await _productRepository.UpdateProductAsync(productToUpdate);
                var updatedKeywords = productToUpdate.Keywords.Select(k => k.KeywordText).ToList();
                return (true, updatedKeywords);
            }
            return (false, null);
        }


        private void UpdateKeywords(Product product, string newKeywordsString)
        {
            var newKeywords = newKeywordsString.Split(',').Select(k => k.Trim()).Distinct().ToList();

            product.Keywords.Clear();
            foreach (var keywordText in newKeywords)
            {
                var keyword = product.Keywords.FirstOrDefault(k => k.KeywordText == keywordText);
                if (keyword == null)
                {
                    keyword = new Keyword { KeywordText = keywordText };
                    product.Keywords.Add(keyword);
                }
            }
        }






        public async Task<Product> CreateProductAsync(string Name, string Keywords, string Description, string Department, string supervisorName, string supervisorLastName, string supervisorDepartment, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment, int currentUserId, int Quantity, Product product)
        {
            //var existingProduct = await _productRepository.GetAllProductsAsync()..FirstOrDefaultAsync(p => p.Name == name);
            var existingProduct = (await _productRepository.GetAllProductsAsync()).FirstOrDefault(p => p.Name == product.Name);
            if (existingProduct != null)
            {
                throw new Exception("Ürün mevcut. Lütfen güncelleyin.");
            }

            if (product.Keywords == null)
            {
                product.Keywords = new List<Keyword>();
            }

            if (!string.IsNullOrEmpty(Keywords))
            {
                var keyword = product.Keywords.FirstOrDefault(k => k.KeywordText == Keywords);

                if (keyword == null)
                {
                    keyword = new Keyword { KeywordText = Keywords };
                    product.Keywords.Add(keyword);
                }
            }

            var supervisor = product.Supervisor;
            if (supervisor == null)
            {
                supervisor = new Supervisor { Name = supervisorName, Surname = supervisorLastName, Department = supervisorDepartment };
                product.Supervisor = supervisor;
            }

            var cosupervisor = product.CoSupervisor;
            if (cosupervisor == null)
            {

                cosupervisor = new Cosupervisor { Name = coSupervisorName, Surname = coSupervisorLastName, Department = coSupervisorDepartment };

                product.CoSupervisor = cosupervisor;
            }
            product.Quantity = Quantity;
            product.UpdatedById = null;

            return await _productRepository.AddProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}