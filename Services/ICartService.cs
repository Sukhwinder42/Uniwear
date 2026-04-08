using System.Security.Claims;
using Uniwear.ViewModels;

public interface ICartService
{
    Task<IEnumerable<CartViewModel>> GetUserCartAsync(ClaimsPrincipal user);

    Task AddToCartAsync(ClaimsPrincipal user, int productId);

    Task RemoveAsync(int cartItemId);
    Task IncreaseQuantity(int cartItemId);
    Task DecreaseQuantity(int cartItemId);
}