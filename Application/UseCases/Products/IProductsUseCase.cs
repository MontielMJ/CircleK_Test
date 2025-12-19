using Application.Dtos;

namespace Application.UseCases.Products;

public interface IProductsUseCase
{
    Task<List<ProductDto>> getProductsSkuName(GetProductsQuery query, CancellationToken cancellationToken = default);
    Task<ProductDto> GetProductById(int id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateProduct(ProductRequest request, CancellationToken cancellationToken = default);
    Task DeleteProduct(int id, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateProduct(int id, ProductRequest request, CancellationToken cancellationToken = default);
}
