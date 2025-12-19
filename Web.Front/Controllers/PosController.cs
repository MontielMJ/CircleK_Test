using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Web.Front.Models;

namespace Web.Front.Controllers
{
    public class PosController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string CART_KEY = "POS_CART";
        private const string SALE_ID_KEY = "SALE_ID";

        public PosController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var model = await GetCart();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            var client = _clientFactory.CreateClient("Api");
            var products = await client.GetFromJsonAsync<List<ProductViewModel>>(
                $"products?search={search}");

            return PartialView("_ProductResults", products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var client = _clientFactory.CreateClient("Api");
            var product = await client.GetFromJsonAsync<ProductViewModel>($"products/{productId}");

            var cart = await GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var cart = await GetCart();
            cart.Items.RemoveAll(i => i.ProductId == productId);
            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SaveSale()
        {
            var cart = await GetCart();
            if (!cart.Items.Any())
                return RedirectToAction(nameof(Index));

            var client = _clientFactory.CreateClient("Api");

            var request = new
            {
                items = cart.Items.Select(i => new
                {
                    productId = i.ProductId,
                    quantity = i.Quantity
                })
            };

            var response = await client.PostAsJsonAsync("sales", request);
            response.EnsureSuccessStatusCode();

            var sale = await response.Content.ReadFromJsonAsync<SaleViewModel>();

            if (response.StatusCode.Equals(HttpStatusCode.OK)) 
            {
                HttpContext.Session.Remove(CART_KEY);
                HttpContext.Session.Remove(SALE_ID_KEY);
                TempData["SuccessMessage"] = $"Venta guardada correctamente. Folio: {sale.Id}";
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Pay(int saleId, int method, decimal amount, string? reference)
        {
            if (saleId == null)
                return RedirectToAction(nameof(Index));

            var client = _clientFactory.CreateClient("Api");

            var request = new
            {
                payments = new[]
                {
                new {
                    method = method,
                    amount,
                    reference
                }
            }
            };

            var response = await client.PostAsJsonAsync($"sales/{saleId}/payments", request);
         
            var result = await response.Content.ReadFromJsonAsync<PayResultViewModel>();

            if (result!.Status == "APPROVED")
            {
                HttpContext.Session.Remove(CART_KEY);
                HttpContext.Session.Remove(SALE_ID_KEY);
            }

            TempData["PAY_RESULT"] = result.Message;
            TempData["AMOUNT_CHANGE"] = result.Change?.ToString("C");
            return RedirectToAction(nameof(Index));
        }

        private async Task<CartViewModel> GetCart()
        {
            string json = HttpContext.Session.GetString(CART_KEY);
            CartViewModel cartView = new CartViewModel();
            
            if (json == null)
            {
                new CartViewModel();
            }
            else
            {
                cartView = JsonSerializer.Deserialize<CartViewModel>(json);

            }
            cartView.ItemsSale = await getSalesSave();
            return cartView;
        }

        private async Task<List<SaleViewModel>> getSalesSave()
        {
            var client = _clientFactory.CreateClient("Api");
            var sales = await client.GetFromJsonAsync<List<SaleViewModel>>($"sales");

            return sales.ToList();
        }


        private void SaveCart(CartViewModel cart)
        {
            HttpContext.Session.SetString(
                CART_KEY,
                JsonSerializer.Serialize(cart));
        }
    }
}
