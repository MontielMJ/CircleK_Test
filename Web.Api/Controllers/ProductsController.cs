using Application.Dtos;
using Application.Handler.Products;
using Application.UseCases.Products;
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
            GetProductsQuery query = new GetProductsQuery
            {
                Search = search,
                Page = 1,
                PageSize = 10
            };

            var products = await _productsUseCase.getProductsSkuName(query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productsUseCase.GetProductById(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequest request)
        {
            ProductDto productDto = await _productsUseCase.CreateProduct(request);
            return productDto != null ? CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto) : BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductRequest request)
        {
            ProductDto productDto = await _productsUseCase.UpdateProduct(id, request);
            return productDto != null ? Ok(productDto) : NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productsUseCase.DeleteProduct(id);
            return NoContent();
        }
    }
}
