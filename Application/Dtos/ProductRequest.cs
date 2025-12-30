using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class ProductRequest
{
    [Required(ErrorMessage = "SKU is required")]
    [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
    [RegularExpression(@"^[A-Z0-9\-_]+$", ErrorMessage = "SKU can only contain uppercase letters, numbers, hyphens and underscores")]
    public string SKU { get; set; } = null!;

    [Required(ErrorMessage = "Product name is required")]
    [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Product name must be at least 3 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock is required")]
    [Range(0, 999999, ErrorMessage = "Stock must be between 0 and 999,999")]
    public int Stock { get; set; }
}
