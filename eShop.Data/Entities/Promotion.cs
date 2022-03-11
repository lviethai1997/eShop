using eShop.Data.Enums;
using System;

namespace eShop.Data.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool ApplyForAll { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string ProductIDs { get; set; }
        public string ProducntCategoryIDs { get; set; }
        public Status Status { get; set; }
        public string Name { get; set; }
    }
}