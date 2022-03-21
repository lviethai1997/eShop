using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string langId,GetPublicProductPagingRequest request);

        Task<List<ProductViewModel>> GetAll(string langId);
    }
}