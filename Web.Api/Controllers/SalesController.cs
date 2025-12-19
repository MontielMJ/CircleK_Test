using Application.Dtos;
using Application.UseCases.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {

        private readonly ISalesUseCase _salesUseCase;
        public SalesController(ISalesUseCase salesUseCase)
        {
           _salesUseCase = salesUseCase;
        }

        [HttpPost("{id}/payments")]
        public async Task<IActionResult> Pay(int id, PaySaleRequest request)
        {

            var result = await _salesUseCase.PaySaleAsync(id, request);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {

            await _salesUseCase.CancelSaleAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSaleRequest request)
        {
            var result = await _salesUseCase.CreateSaleAsync(request);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            var result = await _salesUseCase.GetSalesAsync();
            return Ok(result);
        }
    }
}
