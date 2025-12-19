using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sales
{
    public interface ISalesUseCase
    {
        Task CancelSaleAsync(int saleId, CancellationToken ct = default);
        Task<SaleDto> CreateSaleAsync(CreateSaleRequest request, CancellationToken ct = default);
        Task<PaySaleResult> PaySaleAsync(int saleId, PaySaleRequest request, CancellationToken ct = default);
        Task<List<SaleDto>> GetSalesAsync(); 
    }
}
