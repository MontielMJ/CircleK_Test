using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly PosDbContext _context;
        private readonly PaymentService _paymentService;
        public SalesRepository(PosDbContext context, PaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }
        public async Task CancelSaleAsync(int saleId, CancellationToken ct = default)
        {
var sale = await _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId, ct);

            if (sale == null)
                throw new NotFoundException("Sale", saleId);

if (sale.Status != SaleStatus.Paid)
                throw new BusinessException("Solo se pueden cancelar ventas pagadas");

            foreach (var item in sale.Items)
            {
                item.Product.Stock += item.Quantity;
            }

            sale.Status = SaleStatus.Canceled;
            await _context.SaveChangesAsync(ct);
        }

        public async Task<SaleDto> CreateSaleAsync(CreateSaleRequest sales, CancellationToken ct = default)
        {
if (!sales.Items.Any())
                throw new ValidationException("La venta debe tener al menos un producto");

            var productIds = sales.Items.Select(i => i.ProductId).ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync(ct);

if (products.Count != productIds.Count)
                throw new NotFoundException("Uno o más productos no existen");

            var sale = new Sale
            {
                Folio = GenerateFolio(),
                Status = SaleStatus.Open
            };

            foreach (var item in sales.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);

if (item.Quantity <= 0)
                    throw new ValidationException("Cantidad inválida");

                var saleItem = new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    LineTotal = product.Price * item.Quantity
                };

                sale.Items.Add(saleItem);
            }

            sale.Subtotal = sale.Items.Sum(i => i.LineTotal);
            sale.Tax = CalculateTax(sale.Subtotal);
            sale.Total = sale.Subtotal + sale.Tax;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync(ct);

            return new SaleDto
            {
                Id = sale.Id,
                Folio = sale.Folio,
                Subtotal = sale.Subtotal,
                Tax = sale.Tax,
                Total = sale.Total,
                Status = sale.Status.ToString()
            };
        }

        public async Task<PaySaleResult> PaySaleAsync(int saleId, PaySaleRequest request, CancellationToken ct = default)
        {
            if (request == null)
                return PaySaleResult.Fail("La solicitud de pago es requerida");

            if (request.Payments == null || request.Payments.Count == 0)
                return PaySaleResult.Fail("Debe especificar al menos un método de pago");

            var sale = await _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Include(s => s.Payments)
                .FirstOrDefaultAsync(s => s.Id == saleId, ct);

if (sale == null)
                throw new NotFoundException("Sale", saleId);

            if (sale.Status != SaleStatus.Open && sale.Status != SaleStatus.Pending)
                throw new BusinessException($"La venta no puede ser pagada. Estado actual: {sale.Status}");

            foreach (var item in sale.Items)
            {
                if (item.Product.Stock < item.Quantity)
                {
                    throw new InsufficientStockException(item.Product.Name, item.Quantity, item.Product.Stock);
                }
            }
            decimal totalPaid = sale.Payments
                .Where(p => p.Status == PaymentStatus.Approved)
                .Sum(p => p.Amount);

            decimal newPayments = 0;
            Payment resultPayment = null;
            foreach (var p in request.Payments)
            {
if (p.Amount <= 0)
                    throw new ValidationException("El monto debe ser mayor a cero");

                var payment = new Payment
                {
                    SaleId = sale.Id,
                    Method = p.Method,
                    Amount = p.Amount,
                    Reference = p.Reference,
                    CreatedAt = DateTime.UtcNow
                };
                try
                {
                    resultPayment = _paymentService.ProcessPayment(sale, payment);
                }
catch
                {
                    throw new InvalidPaymentException($"Error procesando pago con método {p.Method}");
                }

                sale.Payments.Add(payment);

                if (payment.Status == PaymentStatus.Approved)
                    newPayments += payment.Amount;
            }
            totalPaid += newPayments;

            if (totalPaid < sale.Total)
            {
                sale.Status = SaleStatus.Pending;
                await _context.SaveChangesAsync(ct);

                return PaySaleResult.Ok(
                    "PENDING",
                    $"Pago insuficiente. Faltan: {(sale.Total - totalPaid):C}"
                );
            }
            else if (totalPaid >= sale.Total)
            {
                sale.Status = SaleStatus.Paid;
                foreach (var item in sale.Items)
                {
                    item.Product.Stock -= item.Quantity;
                }

                await _context.SaveChangesAsync(ct);

                decimal change = 0;
                if (totalPaid > sale.Total)
                {
                    change = totalPaid - sale.Total;
                }

                var lastPayment = sale.Payments
                    .Where(p => p.Status == PaymentStatus.Approved)
                    .LastOrDefault();

                return PaySaleResult.Ok(
                    "APPROVED",
                    "Venta pagada correctamente",
                    lastPayment?.AuthCode,
                    change > 0 ? change : 0
                );
            }

            return PaySaleResult.Fail("Error inesperado");
        }

        private string GenerateFolio()
            => $"V-{DateTime.UtcNow:yyyyMMddHHmmss}";

        private decimal CalculateTax(decimal subtotal)
            => Math.Round(subtotal * 0.16m, 2);

        private decimal CalculateChange(Sale sale)
        {
            var cashPaid = sale.Payments
                .Where(p => p.Method == PaymentMethod.Cash && p.Status == PaymentStatus.Approved)
                .Sum(p => p.Amount);

            return cashPaid > sale.Total ? cashPaid - sale.Total : 0;
        }

public async Task<List<SaleDto>> GetSalesAsync(CancellationToken ct = default)
        {
            return await _context.Sales
                 .Select(s => new SaleDto
                 {
                     Id = s.Id,
                     Folio = s.Folio,
                     Subtotal = s.Subtotal,
                     Tax = s.Tax,
                     Total = s.Total,
                     Status = s.Status.ToString()
                 })
                 .ToListAsync(ct);
        }
    }
}
