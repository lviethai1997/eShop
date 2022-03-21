using eShop.Data.EF;
using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _context;

        public PublicProductService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductViewModel>> GetAll(string langId)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductID
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductID
                        join c in _context.Categories on pic.CategoryID equals c.Id
                        where pt.LanguageID == langId
                        select new { p, pt, pic };
       
            var data = query.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                DateCreated = x.p.CreatedDate,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageID,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount
            }).ToListAsync();

            return await data;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string langId,GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductID
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductID
                        join c in _context.Categories on pic.CategoryID equals c.Id
                        where pt.LanguageID == langId
                        select new { p, pt, pic };

            if (request.CategoryID.HasValue && request.CategoryID.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryID == request.CategoryID);
            }

            int totalRow = await query.CountAsync();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Take(request.PageSize).Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                DateCreated = x.p.CreatedDate,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageID,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount
            }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = await data
            };

            return pageResult;
        }
    }
}