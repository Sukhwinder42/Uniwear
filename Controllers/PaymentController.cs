using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Uniwear.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ICartService _cartService;

        public PaymentController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult CreateCheckoutSession(decimal amount)
        {
            var domain = "https://localhost:7043/";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "inr",
                            UnitAmount = (long)(amount * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Your Order"
                            }
                        },
                        Quantity = 1
                    }
                },

                Mode = "payment",

                SuccessUrl = domain + "Payment/Success",
                CancelUrl = domain + "Payment/Cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        public async Task<IActionResult> Success()
        {
            // Remove all items from the logged-in user's cart
            var cartItems = await _cartService.GetUserCartAsync(User);

            foreach (var item in cartItems)
            {
                await _cartService.RemoveAsync(item.CartItemId);
            }

            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }
      
    }
}
