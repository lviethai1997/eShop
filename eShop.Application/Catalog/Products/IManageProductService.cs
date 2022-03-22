using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newprice);

        Task<bool> UpdateStock(int productId, int newstock);

        Task AddViewCount(int productId);

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImagesCreateRequest request);

        Task<int> DeleteImage(int productId, int imageId);

        Task<int> UpdateImage(int productId, int imageId, ProductImagesUpdateRequest request);

        Task<List<ProductImageViewModel>> GetListImage(int productId);

        Task<ProductViewModel> GetById(int productId, string langId);

        Task<ProductImageViewModel> GetImageById(int productId, int imageId);

        Task<List<ProductImageViewModel>> GetImagesByProductId(int productId);

        Task<int> AddManyImageProduct(int productId, List<IFormFile> images);
    }
}