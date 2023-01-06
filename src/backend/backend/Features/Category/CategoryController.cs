using backend.Features.Product.Models;
using backend.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Features.Category;
/// <summary>
/// API Controller for Inventory grain. An Inventory represents a category, which consists of products
/// </summary>
[ApiController]
[Route("category")]
public class CategoryController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;

    public CategoryController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }
    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>A list of ProductCategory</returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCategory>))]
    public ActionResult GetCategories()
    {
        var categories = EnumHelpers.GetEnumsAsList();
        //var inventoryGrain = _grainFactory.GetGrain<ICategoryGrain>(category.ToString());
        //var products  = await inventoryGrain.GetAllProducts(limit);
        return Ok(categories);
    }
    
    /// <summary>
    /// Get items of a specific category, default is 10
    /// </summary>
    /// <param name="category">Enter a category to fetch items of</param>
    /// <param name="limit">Enter amount, leave empty if default amount should be retrieved</param>
    /// <returns>List of products</returns>
    [HttpGet("{category}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDetail>))]
    public async Task<ActionResult> GetItemsForCategory(ProductCategory category, [FromQuery] int limit = 10)
    {
        var inventoryGrain = _grainFactory.GetGrain<ICategoryGrain>(category.ToString());
        var products  = await inventoryGrain.GetAllProducts(limit);
        return Ok(products);
    }
    
    /// <summary>
    /// Get total items of a random category, default is 10
    /// </summary>
    /// <param name="limit">Enter amount, leave empty if default amount should be retrieved</param>
    /// <returns>List of products</returns>
    [HttpGet("random")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDetail>))]
    public async Task<ActionResult> GetItemsOfRandomCategory([FromQuery] int limit = 10)
    {
        Array values = Enum.GetValues<ProductCategory>();
        var random = new Random();
        var category = (ProductCategory)values.GetValue(random.Next(values.Length));
        var inventoryGrain = _grainFactory.GetGrain<ICategoryGrain>(category.ToString());
        var products = await inventoryGrain.GetAllProducts(limit);
        return Ok(products);
    }
}