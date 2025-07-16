using Inventory.Api.Data;
using Microsoft.Identity.Client;
using ProductInventorySystem.Models;
using System.Data;

namespace Inventory.Api.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetInStockProductsAsync();
        Task AddProductAsync(Product product);
        Task<int> UpdateProductAsync(int id, Product product);
        Task<int> DeleteProductAsync(int id);
    }
}
