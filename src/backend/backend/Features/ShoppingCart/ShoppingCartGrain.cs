using backend.Features.Product;
using backend.Features.Product.Models;
using backend.Features.ShoppingCart.Models;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace backend.Features.ShoppingCart;

[Reentrant]
public class ShoppingCartGrain : Grain, IShoppingCartGrain
{
    private readonly IPersistentState<Dictionary<string, CartItem>> _cart;

    public ShoppingCartGrain([PersistentState(stateName:"shoppingCart", storageName:"shopping-cart")]
        IPersistentState<Dictionary<string, CartItem>> cart)
    {
        _cart = cart;
    }

    public async Task<bool> AddOrUpdateItem(string productId, int quantity)
    {
        var product = GrainFactory.GetGrain<IProductGrain>(productId);
        int? adjustedQuantity = null!;

        if (_cart.State.TryGetValue(productId, out var existingItem))
        {
            adjustedQuantity = quantity - existingItem.Quantity;
        }
        var (available, claimed) = await product.TakeProduct(adjustedQuantity ?? quantity);
        
        if (!available || claimed is null) return false;
        var item = ToCartItem(quantity, claimed);
        _cart.State[claimed.Id] = item;
        await _cart.WriteStateAsync();
        return true;
    }

    public async Task RemoveItem(string productId, int quantity)
    {
        var productGrain = GrainFactory.GetGrain<IProductGrain>(productId);
        await productGrain.ReturnProduct(quantity);

        if (_cart.State.Remove(productId))
        {
            await _cart.WriteStateAsync();
        }
    }

    public Task<HashSet<CartItem>> GetAllItems() => Task.FromResult(_cart.State.Values.ToHashSet());
    public Task<int> GetTotalItems() => Task.FromResult(_cart.State.Count);

    public async Task EmptyCart()
    {
        _cart.State.Clear(); 
        await _cart.ClearStateAsync();
    }

    private CartItem ToCartItem(int quantity, ProductDetail productDetail) =>
        new(this.GetPrimaryKeyString(), quantity, productDetail);
}