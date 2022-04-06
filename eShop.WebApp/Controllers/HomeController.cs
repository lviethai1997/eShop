using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace eShop.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;

        public HomeController(ISharedCultureLocalizer loc,ILogger<HomeController> logger)
        {
            _logger = logger;
            _loc = loc;
        }
        public IActionResult Index()
        {
            var msg = _loc.GetLocalizedString("Vietname");
            return View();
        }

        public IActionResult SetCultureCookie(string cltr,string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = System.DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
                
        }
    }
}
