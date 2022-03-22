using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class RegisterRequestValidation: AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().Length(2, 200).WithMessage("FirstName must beween 6 and 200 chars");
            RuleFor(x => x.LastName).NotEmpty().Length(2, 200).WithMessage("LastName must beween 6 and 200 chars");
            RuleFor(x=>x.DoB).NotEmpty().GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("you so old to use internet");
            RuleFor(x=>x.Email).NotEmpty().EmailAddress().Length(2, 200).Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").WithMessage("Email is incorrect");
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().Length(6,20).WithMessage("Password must beween 6 and 20 chars");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("confirm password is not matches");
                }
            });
        }
    }
}
