using ShopperBackend.Models;
using ShopperBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShopperBackend.Exceptions;

namespace ShopperBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            throw new CustomNotFoundException("This is custom a KeyNotFoundException.");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productsService.GetAsync();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _productsService.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _productsService.CreateAsync(product);
            //HttpContext.Response.Headers.Append("X-Custom-Header", "CustomHeaderValue");
            return StatusCode(201, product);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            var existingProduct = await _productsService.GetAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            await _productsService.UpdateAsync(id, product);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var existingProduct = await _productsService.GetAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            await _productsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
