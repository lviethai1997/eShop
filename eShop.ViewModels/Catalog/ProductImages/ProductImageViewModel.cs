﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Catalog.ProductImages
{
    public class ProductImageViewModel
    {
        public int ProductId { get; set; }
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
    }
}
