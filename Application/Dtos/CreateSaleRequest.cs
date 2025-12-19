namespace Application.Dtos;

public class CreateSaleRequest
{
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}
