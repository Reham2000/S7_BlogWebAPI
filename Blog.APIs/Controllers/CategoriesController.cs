using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        public CategoriesController(ICategoryService category)
        {
            _category = category;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetAll()
        {
            var Categories = await _category.GetAllAsync();
            if (Categories == null || !Categories.Any())
                return new List<Category>();
            return Categories;
        }
    }
}
