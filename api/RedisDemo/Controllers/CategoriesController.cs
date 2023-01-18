using Microsoft.AspNetCore.Mvc;
using RedisDemo.Models;
using RedisDemo.Repositories.Interfaces;

namespace RedisDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync()
    {
        return Ok(await _categoryRepository.GetCategoriesAsync());
    }
}