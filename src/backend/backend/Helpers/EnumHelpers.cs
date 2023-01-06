using backend.Features.Product.Models;

namespace backend.Helpers;

public static class EnumHelpers
{
    public static List<ProductCategory> GetEnumsAsList()
    {
        var categories = Enum.GetValues<ProductCategory>().ToList();
        return categories;
    }
}