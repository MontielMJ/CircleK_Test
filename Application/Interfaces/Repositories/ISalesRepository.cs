using Application.Dtos;

namespace Application.Interfaces.Repositories
{
    public interface ISalesRepository
    {
        Task CancelSaleAsync(int saleId, CancellationToken ct = default);
        Task<SaleDto> CreateSaleAsync(CreateSaleRequest sale, CancellationToken ct = default);
        Task<PaySaleResult> PaySaleAsync(int saleId, PaySaleRequest request, CancellationToken ct = default);
        Task<List<SaleDto>> GetSalesAsync ();
    }
}
