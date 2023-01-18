using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisDemo.Models;
using RedisDemo.Repositories.Interfaces;

namespace RedisDemo.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(DbContext dbContext, IDistributedCache cache, ILogger<CategoryRepository> logger)
    {
        _dbContext = dbContext;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        var categories = new List<Category>();

        string cacheKey = "categories";
        var encodedCategories = await _cache.GetAsync(cacheKey);
        if (encodedCategories is not null)
        {
            string serializedCategories = Encoding.UTF8.GetString(encodedCategories);
            categories = JsonSerializer.Deserialize<List<Category>>(serializedCategories);
            _logger.LogInformation("Retreived categories from the Redis Cache.");
        }
        else
        {
            categories = await _dbContext.Categories.ToListAsync();
            string serializedCategories = JsonSerializer.Serialize(categories);
            encodedCategories = Encoding.UTF8.GetBytes(serializedCategories);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            await _cache.SetAsync(cacheKey, encodedCategories, cacheOptions);

            _logger.LogInformation("Retrieved categories from the database.");
        }

        return categories;
    }
}