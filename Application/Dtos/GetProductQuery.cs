using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class GetProductsQuery
{
    [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
    public string? Search { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
    public int Page { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 10;
}

public class PagedProductsResult
{
    public List<ProductDto> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
