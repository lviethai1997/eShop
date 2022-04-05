using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Catalog.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string langId);
    }
}
