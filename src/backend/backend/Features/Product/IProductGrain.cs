using backend.Features.Product.Models;

namespace backend.Features.Product;

public interface IProductGrain: IGrainWithStringKey
{
    Task<(bool isAvailable, ProductDetail? productDetail)> TakeProduct(int quantity);
    Task<ProductDetail> GetProductDetails();
    Task CreateOrUpdateProduct(ProductDetail productDetail);
    Task<int> GetProductAvailability();
    Task ReturnProduct(int quantity);
}