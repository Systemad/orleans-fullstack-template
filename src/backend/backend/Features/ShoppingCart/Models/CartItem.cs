using backend.Features.Product.Models;

namespace backend.Features.ShoppingCart.Models;

[GenerateSerializer, Immutable]
public record CartItem(string UserId, int Quantity, ProductDetail ProductDetail);