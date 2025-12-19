using Domain.Enums;

namespace Application.Dtos;

public class PaymentRequest
{
    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }
    public string? Reference { get; set; }
}
