using eShop.Data.Entity;

namespace eShop.Data.Entities
{
    public class CategoryTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string LanguageID { get; set; }
        public string SeoAlias { get; set; }
        public Category Category { get; set; }
        public Language Lanquage { get; set; }
    }
}