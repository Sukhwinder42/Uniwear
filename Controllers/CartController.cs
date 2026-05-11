using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Uniwear.Services;

namespace Uniwear.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetUserCartAsync(User);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                await _cartService.AddToCartAsync(User, productId);

                var cart = await _cartService.GetUserCartAsync(User);

                return Json(new
                {
                    success = true,
                    count = cart.Count()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }



        public async Task<IActionResult> Increase(int cartItemId)
        {
            await _cartService.IncreaseQuantity(cartItemId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(int cartItemId)
        {
            await _cartService.DecreaseQuantity(cartItemId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int cartItemId)
        {
            await _cartService.RemoveAsync(cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddMultiple(List<int> selectedProductIds)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (selectedProductIds == null || !selectedProductIds.Any())
            {
                TempData["Error"] = "Please select at least one product.";
                return RedirectToAction("Index", "Product");
            }

            await _cartService.AddMultipleToCartAsync(User, selectedProductIds);

            TempData["Success"] = "Products added to cart successfully!";

            return RedirectToAction("Index"); // goes to Cart page
        }
    }
}
