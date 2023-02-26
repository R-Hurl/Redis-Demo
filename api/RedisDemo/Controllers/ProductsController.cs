using Microsoft.AspNetCore.Mvc;
using RedisDemo.Repositories.Interfaces;

namespace RedisDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productRepository;

        public ProductsController(IProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<RedisDemo.Models.Product>>> GetProductsByCategoryAsync(short categoryId)
        {
            return Ok(await _productRepository.GetProductsByCategoryAsync(categoryId));
        }
    }
}