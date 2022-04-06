using Microsoft.AspNetCore.Mvc;

namespace eShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
