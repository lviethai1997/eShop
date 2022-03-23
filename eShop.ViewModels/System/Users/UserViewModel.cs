using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid Id { get; set; }
    }
}
