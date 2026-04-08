using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uniwear.Models;
using Uniwear.Services;
using Uniwear.ViewModels;

namespace Uniwear.Controllers

{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> ManageProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            {
                var product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    // map other properties here
                };
                await _productService.CreateProductAsync(product);
                return RedirectToAction("ManageProducts");
            }
        }
    }
}
