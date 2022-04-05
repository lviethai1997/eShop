using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetProductPaging(GetManageProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request);

        Task<ProductViewModel> GetById(int productId,string langid);
    }
}
