using Application.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Sale must contain at least one item")
                .Must(items => items.Count > 0).WithMessage("Sale must contain at least one item")
                .Must(items => items.Count <= 50).WithMessage("Sale cannot contain more than 50 items");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateSaleItemRequestValidator());
        }
    }
}