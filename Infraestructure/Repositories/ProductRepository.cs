using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PosDbContext _context;

    public ProductRepository(PosDbContext context)
    {
        _context = context;
    }

public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.SKU == sku, cancellationToken);
    }

public async Task<(List<Product> Products, int TotalCount)> GetProductsAsync(string? search = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.SKU.Contains(search) ||
                p.Name.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }

public async Task<Product?> InsertProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        Product? existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.SKU == product.SKU, cancellationToken);
        if (existingProduct != null)
            return null;
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

public async Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        Product? existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id, cancellationToken);
        if (existingProduct == null)
            return null;

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        await _context.SaveChangesAsync(cancellationToken);
        return existingProduct;
    }
}
