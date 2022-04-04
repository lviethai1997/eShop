using eShop.AdminAplication.Models;
using eShop.AdminAplication.Services;
using eShop.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Controllers.Component
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;

        public NavigationViewComponent(ILanguageApiClient languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(NavigationViewModel model)
        {
            var languages = await _languageApiClient.GetAll();

            var navigationvm = new NavigationViewModel()
            {
                CurrentLanguageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLangId),
                Languages = languages.ResultObject
            };

            return View("Default", navigationvm);
        }
    }
}