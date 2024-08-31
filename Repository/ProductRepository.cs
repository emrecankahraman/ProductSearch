using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductSearch.Models;
using ProductSearch.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProductSearch.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly Database1Context _context;

        public ProductRepository(Database1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Keywords)
                .Include(p => p.Supervisor)
                .Include(p => p.CoSupervisor)
                .Include(p => p.CreatedBy)
                .Include(p => p.UpdatedBy)
                .ToListAsync();

            Console.WriteLine($"Products fetched from DB: {products.Count}");
            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.Name}, Supervisor: {product.Supervisor?.Name}, CoSupervisor: {product.CoSupervisor?.Name}, Keywords: {string.Join(", ", product.Keywords?.Select(k => k.KeywordText) ?? Enumerable.Empty<string>())}");
            }

            return products;
        }
        //public async Task<IEnumerable<Product>> GetProductsWithCreatorInfo()
        //{
        //    var products = await GetAllProductsAsync();
        //    var personIds = new HashSet<int>();

        //    //foreach (var product in products)
        //    //{
        //    //    personIds.Add(product.CreatedById);
        //    //    personIds.Add(product.UpdatedById);
        //    //}

        //    //var persons = await _context.People
        //    //    .Where(p => personIds.Contains(p.Id))
        //    //    .ToDictionaryAsync(p => p.Id);

        //    //foreach (var product in products)
        //    //{
        //    //    product.CreatedBy = persons.TryGetValue(product.CreatedById, out var creator) ? creator : null;
        //    //    product.UpdatedBy = persons.TryGetValue(product.UpdatedById, out var updater) ? updater : null;
        //    //}

        //    return products;
        //}
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Keywords)
                .Include(p => p.Supervisor)
                .Include(p => p.CoSupervisor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
