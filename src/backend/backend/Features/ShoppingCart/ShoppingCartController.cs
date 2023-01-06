using backend.Features.ShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Features.ShoppingCart;

/// <summary>
/// API Controller for shopping cart
/// </summary>
[ApiController]
[Route("shopping-cart")]
public class ShoppingCartController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGrainFactory _grainFactory;
    
    public ShoppingCartController(IGrainFactory grainFactory, IHttpContextAccessor httpContextAccessor)
    {
        _grainFactory = grainFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A list of clients items in shopping cart</returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CartItem>))]
    public async Task<ActionResult> GetShoppingCart()
    {
        var clientSession = _httpContextAccessor.SetOrCreateCookieCartId();
        var cartGrain = _grainFactory.GetGrain<IShoppingCartGrain>(clientSession);
        var items = await cartGrain.GetAllItems();
        return Ok(items);
    }
    
    [AllowAnonymous]
    [HttpGet("add/{id}/{quantity:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CartItem>))]
    public async Task<ActionResult> AddItemToCart(string id, int quantity)
    {
        var clientSession = _httpContextAccessor.SetOrCreateCookieCartId();
        var cartGrain = _grainFactory.GetGrain<IShoppingCartGrain>(clientSession);
        await cartGrain.AddOrUpdateItem(id, quantity);
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("remove/{id}/{quantity:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveItemFromCart(string id, int quantity)
    {
        var clientSession = _httpContextAccessor.SetOrCreateCookieCartId();
        var cartGrain = _grainFactory.GetGrain<IShoppingCartGrain>(clientSession);
        await cartGrain.RemoveItem(id, quantity);
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("empty")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> EmptyCart()
    {
        var clientSession = _httpContextAccessor.SetOrCreateCookieCartId();
        var cartGrain = _grainFactory.GetGrain<IShoppingCartGrain>(clientSession);
        await cartGrain.EmptyCart();
        return Ok();
    }
    
}