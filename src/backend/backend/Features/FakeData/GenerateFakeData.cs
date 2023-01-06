using backend.Features.Product.Models;
using Bogus;

namespace backend.Features.FakeData;

public static class GenerateFakeData
{
    public static Faker<ProductDetail> SetupBogusData(this ProductDetail productDetail)
    {
        var faker = new Faker<ProductDetail>()
            .StrictMode(true)
            .RuleFor(p => p.Id, (f, p) => f.Random.Number(1, 100_000).ToString())
            .RuleFor(p => p.Name, (f, p) => f.Commerce.ProductName())
            .RuleFor(p => p.Description, (f, p) => f.Lorem.Sentence())
            .RuleFor(p => p.Price, (f, p) => decimal.Parse(f.Commerce.Price(max: 170)))
            .RuleFor(p => p.Quantity, (f, p) => f.Random.Number(0, 1_200))
            .RuleFor(p => p.ImageUrl, (f, p) => f.Image.PicsumUrl())
            .RuleFor(p => p.Category, (f, p) => ProductCategory.Music);
            //.RuleFor(p => p.Category, (f, p) => f.PickRandom<ProductCategory>());
            return faker;
    } 
    
    public static bool MatchesFilter(this ProductDetail product, string? filter)
    {
        if (filter is null or { Length: 0 })
        {
            return true;
        }
        
        return product.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
                   || product.Description.Contains(filter, StringComparison.OrdinalIgnoreCase);

    }
}