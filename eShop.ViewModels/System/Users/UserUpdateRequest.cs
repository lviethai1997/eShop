using System;

namespace eShop.ViewModels.System.Users
{
    public class UserUpdateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DoB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string Username { get; set; }
    }
}