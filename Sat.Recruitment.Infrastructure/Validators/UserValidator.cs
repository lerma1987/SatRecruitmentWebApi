using FluentValidation;
using Sat.Recruitment.Core.DTOs;
using System.Text.RegularExpressions;

namespace Sat.Recruitment.Infrastructure.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator() 
        { 
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("FirstName is required");

            RuleFor(user => user.Address)
                .NotEmpty()
                .WithMessage("Address is required");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("Email address is required")
                .EmailAddress().WithMessage("A valid email is required");

            RuleFor(user => user.Phone)
                .NotEmpty()
                .NotNull().WithMessage("Phone Number is required.")
                .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
                .MaximumLength(10).WithMessage("PhoneNumber must not exceed 10 characters.")
                .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("PhoneNumber not valid");

            RuleFor(user => user.UserTypeId)
                .IsInEnum();

            RuleFor(user => user.Money)
                .NotNull()
                .GreaterThan(-1);
        }
    }
}
