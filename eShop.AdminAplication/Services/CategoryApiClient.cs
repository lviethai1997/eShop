using eShop.ViewModels.Catalog.Categories;
using eShop.ViewModels.Catalog.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpContextAccessor, httpClientFactory, configuration)
        {


        }



        public async Task<List<CategoryViewModel>> GetAll(string langId)
        {
            return await GetListAsync<CategoryViewModel>($"api/Categories/GetAllCategories?langId={langId}");
        }
    }
} 
