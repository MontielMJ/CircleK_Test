using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class PaymentRequest
{
    [Required(ErrorMessage = "Payment method is required")]
    public PaymentMethod Method { get; set; }

    [Required(ErrorMessage = "Payment amount is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Payment amount must be between 0.01 and 999,999.99")]
    public decimal Amount { get; set; }

    [StringLength(100, ErrorMessage = "Reference cannot exceed 100 characters")]
    public string? Reference { get; set; }
}
