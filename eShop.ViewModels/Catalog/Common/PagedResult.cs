using System.Collections.Generic;

namespace eShop.ViewModels.Catalog.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}