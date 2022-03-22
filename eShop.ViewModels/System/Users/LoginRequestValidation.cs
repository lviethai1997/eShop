using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class LoginRequestValidation:AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required").Length(10,20).WithMessage("Username length must beween 10 and 20");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").Length(10,20).WithMessage("Password length must beween 10 and 20");
        }
    }
}
