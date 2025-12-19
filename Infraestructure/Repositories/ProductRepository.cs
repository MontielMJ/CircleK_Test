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

    public Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
       Product? product = _context.Products
            .FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        return Task.CompletedTask;

    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = _context.Products
            .FirstOrDefault(p => p.Id == id);
        return Task.FromResult<Product?>(product);
    }

    public async Task<List<Product>> GetProductsAsync(string? search = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.SKU.Contains(search) ||
                p.Name.Contains(search));
        }

        return await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<Product?> InsertProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        Product? existingProduct = _context.Products
            .FirstOrDefault(p => p.SKU == product.SKU);
        if (existingProduct != null)
            return Task.FromResult<Product?>(null);
        _context.Products.Add(product);
        _context.SaveChanges();
        return Task.FromResult<Product?>(product);
    }

    public Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        Product? existingProduct = _context.Products
            .FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct == null)
            return Task.FromResult<Product?>(null);

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        _context.SaveChanges();
        return Task.FromResult<Product?>(existingProduct);

    }
}
