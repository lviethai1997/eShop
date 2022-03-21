using eShop.Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _ipublicProductService;

        public ProductController(IPublicProductService ipublicProductService)
        {
            _ipublicProductService = ipublicProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _ipublicProductService.GetAll();
            return Ok(product);
        }
    }
}
