using backend.Features.FakeData;
using backend.Features.Product;
using backend.Features.Product.Models;
using Orleans.Runtime;

public class SeedData : IStartupTask
{
    private readonly IGrainFactory _grainFactory;

    public SeedData(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public async Task Execute(CancellationToken cancellationToken)
    {
        var faker = new ProductDetail().SetupBogusData();

        foreach (var product in faker.GenerateLazy(50))
        {
            var productGrain = _grainFactory.GetGrain<IProductGrain>(product.Id);
            await productGrain.CreateOrUpdateProduct(product);
        }
    }
}