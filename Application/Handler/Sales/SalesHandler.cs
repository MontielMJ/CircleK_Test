using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.UseCases.Sales;
using Domain.Exceptions;

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
            try
            {
                await _salesRepository.CancelSaleAsync(saleId, ct);
            }
            catch (DomainException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Failed to cancel sale with id {saleId}", ex);
            }
        }

public async Task<SaleDto> CreateSaleAsync(CreateSaleRequest request, CancellationToken ct = default)
        {
            try
            {
                SaleDto sale = await _salesRepository.CreateSaleAsync(request, ct);
                return sale;
            }
            catch (DomainException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Failed to create sale", ex);
            }
        }

public async Task<List<SaleDto>> GetSalesAsync(CancellationToken ct = default)
        {
            try
            {
                List<SaleDto> sales = await _salesRepository.GetSalesAsync();
                return sales;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Failed to retrieve sales", ex);
            }
        }

public async Task<PaySaleResult> PaySaleAsync(int saleId, PaySaleRequest request, CancellationToken ct = default)
        {
            try
            {
                PaySaleResult result = await _salesRepository.PaySaleAsync(saleId, request, ct);
                return result;
            }
            catch (DomainException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Failed to process payment for sale {saleId}", ex);
            }
        }
    }
}
