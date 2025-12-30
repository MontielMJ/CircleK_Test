using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.UseCases.Products;
using Domain.Entities;
using Domain.Exceptions;

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
            try
            {
                var existingProduct = await _productRepository.GetBySkuAsync(request.SKU, cancellationToken);
                if (existingProduct != null)
                    throw new DuplicateSkuException(request.SKU);

                var product = new Product
                {
                    SKU = request.SKU,
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock
                };

                var createdProduct = await _productRepository.InsertProductAsync(product, cancellationToken);
                
                return new ProductDto
                {
                    Id = createdProduct.Id,
                    SKU = createdProduct.SKU,
                    Name = createdProduct.Name,
                    Price = createdProduct.Price,
                    Stock = createdProduct.Stock
                };
            }
            catch (DuplicateSkuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Failed to create product", ex);
            }
        }

public async Task DeleteProduct(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(id, cancellationToken);
                if (existingProduct == null)
                    throw new NotFoundException("Product", id);

                await _productRepository.DeleteProductAsync(id, cancellationToken);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Failed to delete product with id {id}", ex);
            }
        }

public async Task<ProductDto> GetProductById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id, cancellationToken);
                if (product == null)
                    throw new NotFoundException("Product", id);

                return new ProductDto
                {
                    Id = product.Id,
                    SKU = product.SKU,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Failed to get product with id {id}", ex);
            }
        }

public async Task<PagedProductsResult> getProductsSkuName(GetProductsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var (products, totalCount) = await _productRepository.GetProductsAsync(
                    query.Search,
                    query.Page,
                    query.PageSize,
                    cancellationToken);

                var productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    SKU = p.SKU,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock
                }).ToList();

                return new PagedProductsResult
                {
                    Products = productDtos,
                    TotalCount = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize
                };
            }
            catch (Exception ex)
            {
                throw new BusinessException("Failed to retrieve products", ex);
            }
        }

public async Task<ProductDto> UpdateProduct(int id, ProductRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(id, cancellationToken);
                if (existingProduct == null)
                    throw new NotFoundException("Product", id);

                var product = new Product
                {
                    Id = id,
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock
                };

                var updatedProduct = await _productRepository.UpdateProductAsync(product, cancellationToken);
                
                return new ProductDto
                {
                    Id = updatedProduct.Id,
                    SKU = updatedProduct.SKU,
                    Name = updatedProduct.Name,
                    Price = updatedProduct.Price,
                    Stock = updatedProduct.Stock
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Failed to update product with id {id}", ex);
            }
        }
    }
}
