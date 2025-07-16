using Inventory.Api.Data;
using Inventory.Api.Repositories;
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
        private readonly IProductRepository _productRepository;

        public ProductsController(InventoryContext context, ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            _context = context;
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet]
        [Route("in-stock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetInStockProducts()
        {
            var products = await _productRepository.GetInStockProductsAsync();

            if (!products.Any()) return NotFound();

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddProduct(Product product)
        {
            //either assign it a zero or it is zero by defulat
            //(but don't assign any other value, this will throw an exception)            
            await _productRepository.AddProductAsync(product);
            return Ok(product);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product newProduct)
        {
            if (id != newProduct.Id) return BadRequest();

            int rowsAffected = await _productRepository.UpdateProductAsync(id, newProduct);

            if (rowsAffected == 0) return NotFound("There are no records matching this Id ");
            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            int rowsAffected = await _productRepository.DeleteProductAsync(id);

            if (rowsAffected == 0) return NotFound("There are no records matching this Id");
            return Ok("Deleted Successfully");
        }

    }
}
