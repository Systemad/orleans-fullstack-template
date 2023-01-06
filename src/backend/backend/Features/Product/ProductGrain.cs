using backend.Features.Category;
using backend.Features.Product.Models;
using Orleans.Runtime;

namespace backend.Features.Product;

public class ProductGrain : Grain, IProductGrain
{

    private IPersistentState<ProductDetail> _state;

    public ProductGrain([PersistentState(stateName: "Product", storageName:"shopping-cart")] IPersistentState<ProductDetail> state)
    {
        _state = state;
    }

    public async Task<(bool isAvailable, ProductDetail? productDetail)> TakeProduct(int quantity)
    {
        if (_state.State.Quantity < quantity)
            return (false, null);

        var updateState = _state.State with
        {
             Quantity = _state.State.Quantity - quantity
        };

        await UpdateState(updateState);        
        return (true, updateState);
    }

    public Task<ProductDetail> GetProductDetails() => Task.FromResult(_state.State);

    public Task CreateOrUpdateProduct(ProductDetail productDetail) => UpdateState(productDetail);

    public Task<int> GetProductAvailability() => Task.FromResult(_state.State.Quantity);

    public Task ReturnProduct(int quantity) =>
        UpdateState(_state.State with
        {
            Quantity = _state.State.Quantity + quantity
        });
    
    private async Task UpdateState(ProductDetail productDetail)
    {
        var oldCategory = _state.State.Category;
        _state.State = productDetail;
        await _state.WriteStateAsync();
        
        var newInventory = GrainFactory.GetGrain<ICategoryGrain>(_state.State.Category.ToString());
        await newInventory.AddOrUpdateProduct(productDetail);
        
        if (oldCategory != productDetail.Category)
        {
            // TODO: Dirty fix, all lower case requires
            var oldInventory = GrainFactory.GetGrain<ICategoryGrain>(oldCategory.ToString().ToLower());
            await oldInventory.RemoveProduct(productDetail.Id);
        }
    }
}