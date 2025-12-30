using Application.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class PaySaleRequestValidator : AbstractValidator<PaySaleRequest>
    {
        public PaySaleRequestValidator()
        {
            RuleFor(x => x.Payments)
                .NotEmpty().WithMessage("At least one payment is required")
                .Must(payments => payments.Count > 0).WithMessage("At least one payment is required")
                .Must(payments => payments.Count <= 10).WithMessage("Cannot process more than 10 payments at once");

            RuleForEach(x => x.Payments)
                .SetValidator(new PaymentRequestValidator());
        }
    }
}