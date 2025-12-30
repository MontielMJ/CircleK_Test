using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
Task<(List<Product> Products, int TotalCount)> GetProductsAsync(
        string? search = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
        Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<Product?> InsertProductAsync(Product product, CancellationToken cancellationToken = default);
        Task DeleteProductAsync(int id, CancellationToken cancellationToken = default);
    }
}
