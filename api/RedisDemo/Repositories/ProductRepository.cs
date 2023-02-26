using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisDemo.Models;
using RedisDemo.Repositories.Interfaces;

namespace RedisDemo.Repositories
{
    public class ProductRepository : IProductsRepository
    {
        private readonly DbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DbContext dbContext, IDistributedCache cache, ILogger<ProductRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(short categoryId)
        {
            var products = new List<Product>();

            string cacheKey = $"products-category-{categoryId}";
            var encodedProducts = await _cache.GetAsync(cacheKey);
            if (encodedProducts is not null)
            {
                string serializedProducts = Encoding.UTF8.GetString(encodedProducts);
                products = JsonSerializer.Deserialize<List<Product>>(serializedProducts);
                _logger.LogInformation("Retreived products from the Redis Cache.");
            }
            else
            {
                products = await _dbContext.Products.Where(product => product.CategoryId == categoryId).ToListAsync();
                string serializedProducts = JsonSerializer.Serialize(products);
                encodedProducts = Encoding.UTF8.GetBytes(serializedProducts);
                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                await _cache.SetAsync(cacheKey, encodedProducts, cacheOptions);
                _logger.LogInformation("Retreived products from the database.");
            }

            return products;
        }
    }
}