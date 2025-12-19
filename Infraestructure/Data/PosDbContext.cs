using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities;

namespace Infraestructure.Data
{
    public class PosDbContext : DbContext
    {
        public PosDbContext(DbContextOptions<PosDbContext> options)
       : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Sale> Sales => Set<Sale>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(p => p.SKU).IsUnique();
                entity.Property(p => p.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasIndex(s => s.Folio).IsUnique();
                entity.Property(s => s.Total).HasPrecision(18, 2);
            });

            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.Property(i => i.UnitPrice)
              .HasPrecision(18, 2);

                entity.Property(i => i.LineTotal)
                      .HasPrecision(18, 2);

                entity.HasOne(i => i.Sale)
                      .WithMany(s => s.Items)
                      .HasForeignKey(i => i.SaleId);

                entity.HasOne(i => i.Product)
                      .WithMany()
                      .HasForeignKey(i => i.ProductId);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Amount)
                      .HasPrecision(18, 2);

                entity.HasOne(p => p.Sale)
                      .WithMany(s => s.Payments)
                      .HasForeignKey(p => p.SaleId);
            });
        }


    }
}
