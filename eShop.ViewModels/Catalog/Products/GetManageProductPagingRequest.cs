﻿using eShop.ViewModels.Catalog.Common;
using System.Collections.Generic;

namespace eShop.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
        public string LanguageId { get; set; }
    }
}