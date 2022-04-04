using eShop.AdminAplication.Models;
using eShop.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.AdminAplication.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Language(NavigationViewModel model)
        {
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLangId, model.CurrentLanguageId);
            return RedirectToAction("Index");
        }
    }
}
