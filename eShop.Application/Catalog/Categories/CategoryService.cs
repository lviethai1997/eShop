using eShop.Data.EF;
using eShop.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;

        public CategoryService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryViewModel>> GetAll(string langId)
        {
            var cate = from c in _context.Categories
                       join pt in _context.CategoryTranslations on c.Id equals pt.CategoryID
                       where pt.LanguageID == langId
                       select new { c, pt };

            return await cate.Select(x => new CategoryViewModel()
            {
                Id =x.c.Id,
                Name =x.pt.Name
            }).ToListAsync();
        }
    }
}
