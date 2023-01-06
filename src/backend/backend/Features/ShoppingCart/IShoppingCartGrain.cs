using backend.Features.ShoppingCart.Models;

namespace backend.Features.ShoppingCart;

public interface IShoppingCartGrain : IGrainWithStringKey
{
    Task<bool> AddOrUpdateItem(string productId, int quantity);
    Task RemoveItem(string productId, int quantity);
    Task<HashSet<CartItem>> GetAllItems();
    Task<int> GetTotalItems();
    Task EmptyCart();
}