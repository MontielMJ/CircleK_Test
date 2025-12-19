using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.UseCases.Products;
using Domain.Entities;

namespace Application.Handler.Products
{
    public class ProductsHandler : IProductsUseCase
    {
        private readonly IProductRepository _productRepository;
        public ProductsHandler(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> CreateProduct(ProductRequest request, CancellationToken cancellationToken = default)
        {
           ProductDto productDto = new ProductDto();
            await _productRepository.InsertProductAsync(new Product
            {
                SKU = request.SKU,
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            }, cancellationToken).ContinueWith(task =>
            {
                var product = task.Result;
                productDto.Id = product.Id;
                productDto.SKU = product.SKU;
                productDto.Name = product.Name;
                productDto.Price = product.Price;
                productDto.Stock = product.Stock;
            }, cancellationToken);
            return productDto;
        }

        public async Task DeleteProduct(int id, CancellationToken cancellationToken = default)
        {
           await _productRepository.DeleteProductAsync(id, cancellationToken);
        }

        public async Task<ProductDto> GetProductById(int id, CancellationToken cancellationToken = default)
        {
            ProductDto? productDto = await _productRepository.GetByIdAsync(id, cancellationToken)
                .ContinueWith(task =>
                {
                    var product = task.Result;
                    if (product == null) return null;

                    return new ProductDto
                    {
                        Id = product.Id,
                        SKU = product.SKU,
                        Name = product.Name,
                        Price = product.Price,
                        Stock = product.Stock
                    };
                }, cancellationToken);
            if (productDto == null)
                throw new Exception("Product not found");
            return productDto;
        }

        public async Task<List<ProductDto>> getProductsSkuName(GetProductsQuery query, CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.GetProductsAsync(
                query.Search,
                query.Page,
                query.PageSize,
                cancellationToken);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                SKU = p.SKU,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();
        }

        public async Task<ProductDto> UpdateProduct(int id, ProductRequest request, CancellationToken cancellationToken = default)
        {
            await _productRepository.UpdateProductAsync(new Product
            {
                Id = id,
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            }, cancellationToken).ContinueWith(task =>
            {
                var product = task.Result;
                if (product == null)
                    throw new Exception("Producto no encontrado");

                return new ProductDto
                {
                    Id = product.Id,
                    SKU = product.SKU,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock
                };
            }, cancellationToken);
            return await GetProductById(id, cancellationToken);
        }
    }
}
