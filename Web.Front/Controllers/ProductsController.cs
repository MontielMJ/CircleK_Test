using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Web.Front.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var client = _clientFactory.CreateClient("Api");
            var products = await client.GetFromJsonAsync<PagedProductsResult>(
                $"products?search={search}");

            return View(products);
        }

        public IActionResult Create()
        {
            return View(new ProductDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var client = _clientFactory.CreateClient("Api");

            var response = await client.PostAsJsonAsync("products", dto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear producto");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            var product = await client.GetFromJsonAsync<ProductDto>($"products/{id}");

            if (product == null)
                return NotFound();

            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var client = _clientFactory.CreateClient("Api");

            var response = await client.PutAsJsonAsync($"products/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al actualizar producto");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            await client.DeleteAsync($"products/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}
