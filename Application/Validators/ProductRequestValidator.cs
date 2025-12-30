using Application.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required")
                .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9\-_]+$").WithMessage("SKU can only contain uppercase letters, numbers, hyphens and underscores");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MinimumLength(3).WithMessage("Product name must be at least 3 characters")
                .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .LessThanOrEqualTo(999999.99m).WithMessage("Price cannot exceed 999,999.99");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative")
                .LessThanOrEqualTo(999999).WithMessage("Stock cannot exceed 999,999");
        }
    }
}