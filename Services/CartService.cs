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
                    Quantity = c.Quantity,
                    StockQuantity = c.Product.StockQuantity
                })
                .ToListAsync();
        }

        public async Task AddToCartAsync(ClaimsPrincipal user, int productId)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products.FindAsync(productId);

            if (product == null)
                throw new Exception("Product not found");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            int currentQty = existingItem?.Quantity ?? 0;

            if (currentQty >= product.StockQuantity)
                throw new Exception($"Only {product.StockQuantity} item(s) available.");

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task IncreaseQuantity(int cartItemId)
        {
            var item = await _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == cartItemId);

            if (item != null)
            {
                if (item.Quantity < item.Product.StockQuantity)
                {
                    item.Quantity += 1;
                    await _context.SaveChangesAsync();
                }
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

        public async Task AddMultipleToCartAsync(ClaimsPrincipal user, List<int> productIds)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var productId in productIds)
            {
                var existingItem = await _context.CartItems
                    .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

                if (existingItem != null)
                {
                    existingItem.Quantity += 1;
                }
                else
                {
                    _context.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        UserId = userId,
                        Quantity = 1
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
