using Inventory.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInventorySystem.Models;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductsController : Controller
    {
        public readonly InventoryContext _context;
        public ProductsController(InventoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet]
        [Route("in-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetInStockProducts()
        {
            var products = await _context.Products.Where(p => p.Quantity > 0).ToListAsync();

            if (products == null) return NotFound();

            return products;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddProduct(Product product)
        {
            product.Id = 0;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product.Id);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product newProduct)
        {
            if (id != newProduct.Id) return BadRequest();

            var product = await _context.Products.FindAsync(newProduct.Id);

            if (product == null) return NotFound();

            product.Name = newProduct.Name;
            product.Quantity = newProduct.Quantity;
            product.Price = newProduct.Price;
            product.Category = newProduct.Category;

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
