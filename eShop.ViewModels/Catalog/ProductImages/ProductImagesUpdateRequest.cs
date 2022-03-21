using Microsoft.AspNetCore.Http;

namespace eShop.ViewModels.Catalog.ProductImages
{
    public class ProductImagesUpdateRequest
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public int SortOrder { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}