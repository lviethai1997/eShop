using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
        

       
    }
}
