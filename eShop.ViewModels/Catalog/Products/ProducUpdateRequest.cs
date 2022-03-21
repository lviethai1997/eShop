﻿using Microsoft.AspNetCore.Http;

namespace eShop.ViewModels.Catalog.Products.Manage
{
    public class ProducUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string LanguageID { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}