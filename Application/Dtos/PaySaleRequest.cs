using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class PaySaleRequest
{
    [Required(ErrorMessage = "Payments are required")]
    [MinLength(1, ErrorMessage = "At least one payment is required")]
    [MaxLength(10, ErrorMessage = "Cannot exceed 10 payments")]
    public List<PaymentRequest> Payments { get; set; } = new();
}
