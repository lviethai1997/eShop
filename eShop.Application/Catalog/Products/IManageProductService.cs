using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Catalog.Products.Manage;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProducUpdateRequest request);

        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newprice);

        Task<bool> UpdateStock(int productId, int newstock);

        Task AddViewCount(int productId);

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<int> AddImage(int productId, List<IFormFile> files);

        Task<int> DeleteImage(int imageId);

        Task<int> UpdateImage(int imageId, string caption, bool isDefault, IFormFile file);

        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}