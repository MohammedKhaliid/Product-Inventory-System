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
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(InventoryContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
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

            if (!products.Any()) return NotFound();

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddProduct(Product product)
        {
            //either assign it a zero or it is zero by defulat
            //(but don't assign any other value, this will throw an exception)
            product.Id = 0; 
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product newProduct)
        {
            if (id != newProduct.Id) return BadRequest();

            int rowsAffected = await _context.Products.Where(p => p.Id == id).ExecuteUpdateAsync(
                setters => setters
                .SetProperty(p => p.Name, newProduct.Name)
                .SetProperty(p => p.Quantity, newProduct.Quantity)
                .SetProperty(p => p.Price, newProduct.Price)
                .SetProperty(p => p.Category, newProduct.Category));

            if (rowsAffected == 0) return NotFound("There are no records matching this Id ");
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            int rowsAffected = await _context.Products.Where(p => p.Id == id).ExecuteDeleteAsync();

            if (rowsAffected == 0) return NotFound("There are no records matching this Id");
            return Ok("Deleted Successfully");
        }

    }
}
