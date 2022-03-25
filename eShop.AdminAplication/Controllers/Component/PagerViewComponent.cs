using eShop.ViewModels.Catalog.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Controllers.Component
{
    public class PagerViewComponent :ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
