using eShop.Data.EF;
using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _config;
        private readonly EShopDbContext _context;

        public LanguageService(IConfiguration configuration,EShopDbContext context)
        {
            _config = configuration;
            _context = context;
        }

        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            var languages =  await _context.Languages.Select(x => new LanguageViewModel() { 
             Id = x.Id,
             Name = x.Name,
            }).ToListAsync();
            return new ApiSuccessResult<List<LanguageViewModel>>(languages);
        }
    }
}
