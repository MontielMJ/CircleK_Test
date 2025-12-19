namespace Application.Dtos;

public class PaySaleRequest
{
    public List<PaymentRequest> Payments { get; set; } = new();
}
