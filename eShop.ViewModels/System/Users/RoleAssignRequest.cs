using eShop.ViewModels.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class RoleAssignRequest
    {
        public Guid id { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
