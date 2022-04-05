using eShop.AdminAplication.Services;
using eShop.Application.Catalog.Products;
using eShop.Utilities.Constants;
using eShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;

        public ProductController(IProductApiClient productApiClient,IConfiguration configuration)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string keyword = null)
        {
            var language = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLangId);

            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = language
            };

            var data = await _productApiClient.GetProductPaging(request);
            ViewBag.keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
               TempData["result"] = "Success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error");
            return View(request);
        }
    }
}
