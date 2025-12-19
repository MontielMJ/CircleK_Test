using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.UseCases.Sales;

namespace Application.Handler.Sales
{
    public class SalesHandler : ISalesUseCase
    {
        private readonly ISalesRepository _salesRepository;
        public SalesHandler(ISalesRepository salesRepository)
        {
             _salesRepository=salesRepository;
        }
        public async Task CancelSaleAsync(int saleId, CancellationToken ct = default)
        {
            await _salesRepository.CancelSaleAsync(saleId, ct);
        }

        public async Task<SaleDto> CreateSaleAsync(CreateSaleRequest request, CancellationToken ct = default)
        {
            SaleDto sale = await _salesRepository.CreateSaleAsync(request, ct);
            return sale;
        }

        public async Task<List<SaleDto>> GetSalesAsync()
        {
            List<SaleDto> sales = await _salesRepository.GetSalesAsync();
            return sales;
        }

        public Task<PaySaleResult> PaySaleAsync(int saleId, PaySaleRequest request, CancellationToken ct = default)
        {
            return _salesRepository.PaySaleAsync(saleId, request, ct);
        }
    }
}
