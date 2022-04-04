using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Languages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public class LanguageApiClient :BaseApiClient,ILanguageApiClient
    {
 

        public LanguageApiClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration)
            :base(httpContextAccessor, httpClientFactory, configuration)
        {
           
        }

        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<LanguageViewModel>>>("/api/Language/GetAll");
        }
    }
}
