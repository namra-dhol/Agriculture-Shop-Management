using FluentValidation;
using AgriShop.Models;


namespace AgriShop.ValidationClass
{
    public class ProductTypeValidator: AbstractValidator<ProductType>
    {
        public ProductTypeValidator()
        {
            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Type Name is required.")
                .MaximumLength(100).WithMessage("Type Name must not exceed 100 characters.")
                .Matches("^[a-zA-Z ]+$").WithMessage("Type Name must contain only letters and spaces.");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required.")
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            // Optional: You can validate dates if needed, e.g., not a future date.
            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.CreatedAt.HasValue)
                .WithMessage("CreatedAt cannot be a future date.");

            RuleFor(x => x.ModifiedAt)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.ModifiedAt.HasValue)
                .WithMessage("ModifiedAt cannot be a future date.");
        }
    }
}
