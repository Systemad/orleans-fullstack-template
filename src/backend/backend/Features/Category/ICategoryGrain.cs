using backend.Features.Product.Models;

namespace backend.Features.Category;

public interface ICategoryGrain : IGrainWithStringKey
{
    Task AddOrUpdateProduct(ProductDetail productDetail);
    Task RemoveProduct(string id);
    Task<HashSet<ProductDetail>> GetAllProducts(int limit);
}