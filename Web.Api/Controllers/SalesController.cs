using Application.Dtos;
using Application.UseCases.Sales;
using Domain.Exceptions;
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
            try
            {
                var result = await _salesUseCase.PaySaleAsync(id, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidPaymentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

[HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _salesUseCase.CancelSaleAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

[HttpPost]
        public async Task<IActionResult> Create(CreateSaleRequest request)
        {
            try
            {
                var result = await _salesUseCase.CreateSaleAsync(request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InsufficientStockException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }
[HttpGet]
        public async Task<IActionResult> GetSales()
        {
            try
            {
                var result = await _salesUseCase.GetSalesAsync();
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }
    }
}
