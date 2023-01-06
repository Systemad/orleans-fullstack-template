using backend.Features.Product;
using backend.Features.Product.Models;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace backend.Features.Category;

[Reentrant]
public class CategoryGrain : Grain, ICategoryGrain
{
    private IPersistentState<CategoryState> _state;
    private readonly Dictionary<string, ProductDetail> _cache = new();

    public CategoryGrain([PersistentState(stateName: "Inventory", "shopping-cart")] IPersistentState<CategoryState> state)
    {
        _state = state;
    }

    private string GrainKey => this.GetPrimaryKeyString(); 
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await SeedCache();
        await base.OnActivateAsync(cancellationToken);
    }
    
    public async Task AddOrUpdateProduct(ProductDetail productDetail)
    {
        _state.State.ProductIds.Add(productDetail.Id);
        _cache[productDetail.Id] = productDetail;
        //_state.State.ItemsAmount = _cache.Count;
        await _state.WriteStateAsync();
    }

    public async Task RemoveProduct(string id)
    {
        _state.State.ProductIds.Remove(id);
        _cache.Remove(id);
        //_state.State.ItemsAmount = _cache.Count;
        await _state.WriteStateAsync();
    }

    public Task<HashSet<ProductDetail>> GetAllProducts(int limit)
    {
        var products = _cache.Values.Take(limit).ToHashSet();
        return Task.FromResult(products);
    }

    private async Task SeedCache()
    {
        if (_state is not { State.ProductIds.Count: > 0  })
        {
            return;
        }
        
        await Parallel.ForEachAsync(_state.State.ProductIds, async (id, _) =>
        {
            var productGrain = GrainFactory.GetGrain<IProductGrain>(id);
            _cache[id] = await productGrain.GetProductDetails();
        });
    }
}