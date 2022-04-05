using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _IhttpContextAccessor;

        public ProductApiClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _IhttpContextAccessor = httpContextAccessor;
        }
        public async Task<PagedResult<ProductViewModel>> GetProductPaging(GetManageProductPagingRequest request)
        {
            var session = _IhttpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"api/product/Paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&languageId={request.LanguageId}");
            var body = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<ProductViewModel>>(body);
        }

      
    }
}
