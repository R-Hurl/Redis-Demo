
using RedisDemo.Models;

namespace RedisDemo.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(short categoryId);
        Task<Product> GetProductByIdAsync(int id);
    }
}