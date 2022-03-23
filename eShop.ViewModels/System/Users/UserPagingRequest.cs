using eShop.ViewModels.Catalog.Common;

namespace eShop.ViewModels.System.Users
{
    public class UserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}