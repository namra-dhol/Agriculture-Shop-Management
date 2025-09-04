using AgriShop.Models;
using FluentValidation;

namespace AgriShop.ValidationClass
{
    public class ProductVarientValidator : AbstractValidator<ProductVariant>
    {
        public ProductVarientValidator() 
        {
            RuleFor(x => x.ProductId)
               .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Size)
                .NotEmpty().WithMessage("Size is required.")
                .MaximumLength(50).WithMessage("Size must not exceed 50 characters.")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Size can only contain letters, numbers, and spaces.");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required.")
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.CreatedAt.HasValue)
                .WithMessage("CreatedAt cannot be a future date.");

            RuleFor(x => x.ModifiedAt)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.ModifiedAt.HasValue)
                .WithMessage("ModifiedAt cannot be a future date.");
        }
        
    }
}
