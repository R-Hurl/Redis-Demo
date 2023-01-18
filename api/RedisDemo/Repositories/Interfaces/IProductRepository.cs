
using RedisDemo.Models;

namespace RedisDemo.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(short categoryId);
        Task<Product> GetProductByIdAsync(int id);
    }
}