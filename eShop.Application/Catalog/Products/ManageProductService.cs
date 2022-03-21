using eShop.Application.Common;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Entity;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Catalog.Products.Manage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShop.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, List<IFormFile> files)
        {
            foreach (var item in files)
            {
                var productImage = new ProductImage()
                {
                    ProductId = productId,
                    Caption = "Thumbnail Image",
                    CreatedDate = DateTime.Now,
                    FileSize = item.Length,
                    ImagePath = await this.SaveFile(item),
                };
                _context.ProductImages.Add(productImage);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                CreatedDate = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageID = request.LanguageID
                    }
                }
            };
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image",
                        CreatedDate = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder =1
                    }
                };
            }

            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var products = await _context.Products.FindAsync(productId);
            if (products == null)
            {
                throw new EShopException($"Cant find Product:{productId}");
            }

            var Images = _context.ProductImages.Where(x => x.ProductId == productId);
            foreach (var item in Images)
            {
                await _storageService.DeleteFileAsync(item.ImagePath);
            }

            _context.Products.Remove(products);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteImage(int imageId)
        {
            var getImage = await _context.ProductImages.FindAsync(imageId);
            if (getImage != null)
            {
                await _storageService.DeleteFileAsync(getImage.ImagePath);
                _context.ProductImages.Remove(getImage);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductID
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductID
                        join c in _context.Categories on pic.CategoryID equals c.Id
                        where pt.Name.Contains(request.keyword)
                        select new { p, pt, pic };
            if (!string.IsNullOrEmpty(request.keyword))
                query = query.Where(x => x.pt.Name.Contains(request.keyword));

            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryID));
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

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var images = await _context.ProductImages.Where(x => x.ProductId == productId).Select(x => new ProductImageViewModel()
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                Caption = x.Caption,
                IsDefault = x.IsDefault,
                FileSize = x.FileSize,
                SortOrder = x.SortOrder,
                CreatedDate = x.CreatedDate
            }).ToListAsync();

            return images;
        }

        public async Task<int> Update(ProducUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranlastions = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductID == request.Id && x.LanguageID == request.LanguageID);
            if (product == null || productTranlastions == null)
            {
                throw new Exception($"Can't find a product with id:{request.Id}");
            }

            productTranlastions.Name = request.Name;
            productTranlastions.Description = request.Description;
            productTranlastions.SeoAlias = request.SeoAlias;
            productTranlastions.SeoDescription = request.SeoDescription;
            productTranlastions.SeoTitle = request.SeoTitle;
            productTranlastions.Details = request.Details;

            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, string caption, bool isDefault, IFormFile file)
        {
            var getImage = await _context.ProductImages.FindAsync(imageId);

            if (getImage != null)
            {
                getImage.Caption = caption;
                getImage.IsDefault = isDefault;
                getImage.FileSize = file.Length;
                getImage.ImagePath = await this.SaveFile(file);
                _context.ProductImages.Update(getImage);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newprice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception($"Can't find a product with id:{productId}");
            }

            product.Price = newprice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int newstock)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception($"Can't find a product with id:{productId}");
            }

            product.Stock = newstock;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFilename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFilename)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}