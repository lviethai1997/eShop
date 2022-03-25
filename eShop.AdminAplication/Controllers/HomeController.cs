using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.AdminAplication.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
