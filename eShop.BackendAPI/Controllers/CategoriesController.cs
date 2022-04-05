using eShop.Application.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromQuery]string langId)
        {
            var cate = await _categoryService.GetAll(langId);

            return Ok(cate);
        }


    }
}
