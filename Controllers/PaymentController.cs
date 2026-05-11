using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Uniwear.Services;


namespace Uniwear.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ICartService _cartService;
        private readonly OrderService _orderService;
        public PaymentController(ICartService cartService, OrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
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
            await _orderService.PlaceOrderAsync(User);   

            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }
      
    }
}
