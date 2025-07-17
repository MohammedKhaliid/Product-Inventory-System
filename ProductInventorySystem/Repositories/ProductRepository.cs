using Inventory.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProductInventorySystem.Models;
using System.Data;

namespace Inventory.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryContext _context;

        public ProductRepository(InventoryContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<IEnumerable<Product>> GetInStockProductsAsync()
        {
            return await _context.Products.Where(p => p.Quantity > 0).ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            product.Id = 0;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateProductAsync(int id, Product newProduct)
        {
            return await _context.Products.Where(p => p.Id == id).ExecuteUpdateAsync(
                setters => setters
                .SetProperty(p => p.Name, newProduct.Name)
                .SetProperty(p => p.Quantity, newProduct.Quantity)
                .SetProperty(p => p.Price, newProduct.Price)
                .SetProperty(p => p.Category, newProduct.Category));
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id).ExecuteDeleteAsync();
        }
    }
}
