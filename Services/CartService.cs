using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Uniwear.Data;
using Uniwear.Models;
using Uniwear.ViewModels;

namespace Uniwear.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartViewModel>> GetUserCartAsync(ClaimsPrincipal user)
        {
            // REMOVED int.Parse
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .Select(c => new CartViewModel
                {
                    ImageUrl = c.Product.ImageUrl,
                    CartItemId = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    Quantity = c.Quantity
                })
                .ToListAsync();
        }

        public async Task AddToCartAsync(ClaimsPrincipal user, int productId)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var productExists = await _context.Products.AnyAsync(p => p.Id == productId);

            if (!productExists)
                throw new Exception("Product not found");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                var cartItem = new CartItem
                {
                  
                    ProductId = productId,
                    UserId = userId,
                    Quantity = 1
                };

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task IncreaseQuantity(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);

            if (item != null)
            {
                item.Quantity += 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DecreaseQuantity(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity -= 1;
                }
                else
                {
                    _context.CartItems.Remove(item);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
