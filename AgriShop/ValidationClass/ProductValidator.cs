using FluentValidation;
using AgriShop.Models;

namespace AgriShop.ValidationClass
{
    public class ProductValidator: AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName)
                .NotNull().WithMessage("Product name must not be empty.")
                .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

            RuleFor(p => p.ProductTypeId)
                .NotEmpty().WithMessage("Product type is required.")
                .GreaterThan(0).WithMessage("Product type must be selected.");

            RuleFor(p => p.SupplierId)
                .NotEmpty().WithMessage("Supplier is required.")
                .GreaterThan(0).WithMessage("Supplier must be selected.");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).When(p => p.Stock.HasValue)
                .WithMessage("Stock must be a non-negative number.");

            RuleFor(p => p.ProductImg)
                .MaximumLength(255).WithMessage("Image URL can't exceed 255 characters.")
                .Matches(@"^.*\.(jpg|jpeg|png|gif|bmp|webp)$").When(p => !string.IsNullOrEmpty(p.ProductImg))
                .WithMessage("Image must be a valid image file (jpg, jpeg, png, gif, bmp, webp).");

            // Optional: Validate CreatedAt and ModifiedAt format if needed, or skip as they are system-generated
        }
    }
}
