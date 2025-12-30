using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class CreateSaleRequest
{
    [Required(ErrorMessage = "Items are required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    [MaxLength(50, ErrorMessage = "Cannot exceed 50 items")]
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}
