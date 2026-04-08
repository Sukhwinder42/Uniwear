using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Uniwear.Data;
using Uniwear.Models;
using Uniwear.Services;
using Uniwear.ViewModels;

namespace Uniwear.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;   
        private readonly ICategoryService _categoryService;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductService productService, ICategoryService categoryService, ApplicationDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        { 
            var products = await _productService.GetAllProductsAsync();

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlistIds = new List<int>();

            if (userId != null)
            {
                wishlistIds = await _context.WishlistItems
                    .Where(w => w.UserId == userId)
                    .Select(w => w.ProductId)
                    .ToListAsync();
            }

            ViewBag.Wishlist = wishlistIds;

            //  GROUP PRODUCTS
            ViewBag.Men = products.Where(p => p.GenderGroup == "Men").ToList();
            ViewBag.Women = products.Where(p => p.GenderGroup == "Ladies").ToList();
            ViewBag.Kids = products.Where(p => p.GenderGroup == "Kids").ToList();

            return View(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ProductViewModel
            {
                Categories = await _categoryService.GetAllCategoriesAsync()
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        { 
            string? uniqueFileName = null;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = uniqueFileName != null ? "/images/" + uniqueFileName : null,
                CategoryId = model.CategoryId,
                StockQuantity = model.StockQuantity,

                GenderGroup = model.GenderGroup
            };

            await _productService.CreateProductAsync(product);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                StockQuantity = product.StockQuantity
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId,
                StockQuantity = model.StockQuantity
            };

            await _productService.UpdateProductAsync(product);

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ProductViewModel model)
        {
            await _productService.DeleteProductAsync(model.Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var products = await _productService.GetByCategoryAsync(categoryId);
            return View("Index", products);
        }

        public async Task<IActionResult> Group(string group)
        {
            var products = await _productService.GetAllProductsAsync();

            // Filter by GenderGroup
            var filteredProducts = products
                .Where(p => !string.IsNullOrEmpty(p.GenderGroup) &&
                            p.GenderGroup.Equals(group, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Categories for the left filter
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;

            // Wishlist for the heart icon
            var wishlistIds = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                wishlistIds = await _context.WishlistItems
                    .Where(w => w.UserId == userId)
                    .Select(w => w.ProductId)
                    .ToListAsync();
            }
            ViewBag.Wishlist = wishlistIds;

            ViewBag.CurrentGroup = group;

            return View("Index", filteredProducts);
        }


        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            var products = await _productService.GetAllProductsAsync();

            var result = products
                .Where(p => !string.IsNullOrEmpty(p.Name) &&
                            p.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name
                })
                .Take(5)
                .ToList();

            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> Filter(int categoryId, decimal maxPrice, string sort, string categories)
        {
            var products = _context.Products.AsQueryable();

            if (categoryId != 0)
                products = products.Where(p => p.CategoryId == categoryId);

            products = products.Where(p => p.Price <= maxPrice);

            products = sort switch
            {
                "low" => products.OrderBy(p => p.Price),
                "high" => products.OrderByDescending(p => p.Price),
                _ => products
            };

            
            var result = await products
    .Select(p => new ProductViewModel
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        ImageUrl = p.ImageUrl, // adjust based on your model
        CategoryId = p.CategoryId
    })
    .ToListAsync();
            return PartialView("_ProductListPartial", result);
        }

    }

}


