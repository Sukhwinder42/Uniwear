using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Uniwear.Data;
using Uniwear.Models;
using Microsoft.AspNetCore.Authorization;

namespace Uniwear.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var items = await _context.WishlistItems
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .ToListAsync();

            return View(items);
        }

        [Authorize]
        public async Task<IActionResult> Toggle(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);

            if (item == null)
            {
              
                _context.WishlistItems.Add(new WishlistItem
                {
                    ProductId = productId,
                    UserId = userId
                });
            }
            else
            {
                _context.WishlistItems.Remove(item);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Product");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the wishlist item
            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
