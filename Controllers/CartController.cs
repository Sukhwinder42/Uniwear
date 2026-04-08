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
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    success = false,
                    redirect = Url.Page("/Account/Login", new { area = "Identity" })
                });
            }

            await _cartService.AddToCartAsync(User, productId);

            var cart = await _cartService.GetUserCartAsync(User);

            return Json(new
            {
                success = true,
                count = cart.Count()
            });
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
    }
}
