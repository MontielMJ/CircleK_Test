using Application.Dtos;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(x => x.Method)
                .IsInEnum().WithMessage("Invalid payment method");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Payment amount must be greater than 0")
                .LessThanOrEqualTo(999999.99m).WithMessage("Payment amount cannot exceed 999,999.99");

            RuleFor(x => x.Reference)
                .MaximumLength(100).WithMessage("Reference cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Reference));
        }
    }
}