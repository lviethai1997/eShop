using eShop.ViewModels.Catalog.Common;
using System.Collections.Generic;

namespace eShop.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string keyword { get; set; }
        public List<int> CategoryIds { get; set; }

    }
}