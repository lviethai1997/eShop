using eShop.Application.Common;
using eShop.Data.EF;
using eShop.Data.Entities;
using eShop.Data.Entity;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;
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
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImagesCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                CreatedDate = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder,
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> AddManyImageProduct(int productId, List<IFormFile> images)
        {
            if (images == null)
            {
                throw new EShopException("no image has been found!");
            }

            foreach (var item in images)
            {
                var productImage = new ProductImage()
                {
                    Caption = "Image for ProductId" + productId,
                    CreatedDate = DateTime.Now,
                    ProductId = productId,
                };

                productImage.ImagePath = await this.SaveFile(item);
                productImage.FileSize = item.Length;

                _context.ProductImages.Add(productImage);
            }
            await _context.SaveChangesAsync();
            return images.Count;
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
                Name = request.Name,
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
                        Caption = "ThumbnailImage",
                        CreatedDate = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder =1
                    }
                };
            }

            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync();
            return product.Id;
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

        public async Task<int> DeleteImage(int productId, int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(productId);

            if (productImage != null)
            {
                await _storageService.DeleteFileAsync(productImage.ImagePath);
                _context.ProductImages.Remove(productImage);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductID
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductID into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryID equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageID == request.LanguageId && pi.IsDefault == true
                        select new { p, pt, pic, pi };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            if (request.CategoryId != null)
            {
                query = query.Where(p => p.pic.CategoryID == request.CategoryId);
            }

            int totalRow = await query.CountAsync();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new ProductViewModel()
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
                ViewCount = x.p.ViewCount,
            }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data
            };

            return pageResult;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductID == productId
            && x.LanguageID == languageId);

            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryID
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryID
                                    where pic.ProductID == productId && ct.LanguageID == languageId
                                    select ct.Name).ToListAsync();

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.CreatedDate,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageID,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories
            };
            return productViewModel;
        }

        public async Task<ProductImageViewModel> GetImageById(int productId, int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null)
            {
                throw new EShopException($"Can't find an image with id:{imageId}");
            }
            var productImageViewModel = new ProductImageViewModel()
            {
                Id = productImage.Id,
                Caption = productImage.Caption,
                CreatedDate = productImage.CreatedDate,
                IsDefault = productImage.IsDefault,
                ImagePath = productImage.ImagePath,
                SortOrder = productImage.SortOrder,
                FileSize = productImage.FileSize,
                ProductId = productImage.ProductId
            };
            return productImageViewModel;
        }

        public async Task<List<ProductImageViewModel>> GetImagesByProductId(int productId)
        {
            var images = await _context.ProductImages.Where(x => x.ProductId == productId).Select(x => new ProductImageViewModel()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ImagePath = x.ImagePath,
                Caption = x.Caption,
                IsDefault = x.IsDefault,
                FileSize = x.FileSize,
                SortOrder = x.SortOrder,
            }).ToListAsync();

            return images;
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

        public async Task<int> Update(ProductUpdateRequest request)
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

        public async Task<int> UpdateImage(int productId, int imageId, ProductImagesUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null)
            {
                throw new EShopException($"Can't find an image with id:{imageId}");
            }

            if (request.ImageFile != null)
            {
                productImage.ProductId = productId;
                productImage.Caption = request.Caption;
                productImage.IsDefault = request.IsDefault;
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Update(productImage);
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

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string langId, GetPublicProductPagingRequest request)
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
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = await data
            };

            return pageResult;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int productId, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(productId);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Can't find product");
            }
            var removeCategory = request.Categories.Where(x => x.Selected == false).ToList();
            //await _userManager.RemoveFromRolesAsync(user, removeRoles);
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories.
                    FirstOrDefaultAsync(x => x.CategoryID == int.Parse(category.Id) && x.ProductID == productId);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected == true)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory() { 
                        CategoryID = int.Parse(category.Id),
                        ProductID = productId,

                    });
                }
            }

            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }
    }
}