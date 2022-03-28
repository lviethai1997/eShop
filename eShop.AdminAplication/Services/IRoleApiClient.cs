using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RolesViewModel>>> GetAll();

    }
}
