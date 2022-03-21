using System.Collections.Generic;

namespace eShop.ViewModels.Catalog.Common
{
    public class PagedResult<T>
    {
        public int TotalRecord { get; set; }
        public List<T> Items { set; get; }
    }
}