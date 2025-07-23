using FluentValidation;
using AgriShop.Models;

namespace AgriShop.ValidationClass
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.UserName)
                .NotNull().WithMessage("User name must not be empty.")
                .Length(3, 50).WithMessage("User name must be between 3 and 50 characters.")
                .Matches("^[A-Za-z ]*$").WithMessage("User name must contain only letters and spaces.");

            RuleFor(u => u.Email)
                .NotNull().WithMessage("Email must not be empty.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(u => u.Password)
                .NotNull().WithMessage("Password must not be empty.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(u => u.Address)
                .NotNull().WithMessage("Address must not be empty.")
                .Length(5, 100).WithMessage("Address must be between 5 and 100 characters.");

            RuleFor(u => u.Phone)
                .NotNull().WithMessage("Phone must not be empty.")
                .Matches("^[0-9]{10,15}$").WithMessage("Phone must be between 10 and 15 digits.");

            RuleFor(u => u.Role)
                .NotNull().WithMessage("Role must not be empty.")
                .Length(3, 20).WithMessage("Role must be between 3 and 20 characters.");
        }
    }
}
