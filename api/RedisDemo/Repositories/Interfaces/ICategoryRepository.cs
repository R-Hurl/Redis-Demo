using RedisDemo.Models;

namespace RedisDemo.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
}