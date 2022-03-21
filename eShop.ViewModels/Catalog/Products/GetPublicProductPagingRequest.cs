using eShop.ViewModels.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest: PagingRequestBase
    {
        public int? CategoryID { get; set; }
    }
}
