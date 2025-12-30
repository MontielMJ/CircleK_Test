using Application.Dtos;
using Application.Handler.Products;
using Application.UseCases.Products;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsUseCase _productsUseCase;

        public ProductsController(IProductsUseCase productsUseCase)
        {
            _productsUseCase = productsUseCase;

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? search)
        {
            try
            {
                GetProductsQuery query = new GetProductsQuery
                {
                    Search = search,
                    Page = 1,
                    PageSize = 10
                };
                
                var result = await _productsUseCase.getProductsSkuName(query);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productsUseCase.GetProductById(id);
                return Ok(product);
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
        public async Task<IActionResult> Create(ProductRequest request)
        {
            try
            {
                ProductDto productDto = await _productsUseCase.CreateProduct(request);
                return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (DuplicateSkuException ex)
            {
                return Conflict(new { error = ex.Message });
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductRequest request)
        {
            try
            {
                ProductDto productDto = await _productsUseCase.UpdateProduct(id, request);
                return Ok(productDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ValidationException ex)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productsUseCase.DeleteProduct(id);
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
    }
}
