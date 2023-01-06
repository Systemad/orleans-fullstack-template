using Microsoft.Net.Http.Headers;

namespace backend.Features.ShoppingCart;

internal static class HttpContextExtensions
{
    internal static string SetOrCreateCookieCartId(this IHttpContextAccessor httpContextAccessor)
    {
        var cartKey = "temp_cartId";
        var contextRequest = httpContextAccessor?.HttpContext;
        if (contextRequest.Request.Cookies[cartKey] != null)
        {
            return contextRequest.Request.Cookies[cartKey];

        }
        Guid cookieId = Guid.NewGuid();
        var options = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1)
        };
        contextRequest.Response.Cookies.Append(cartKey, cookieId.ToString());
        return cookieId.ToString();
    }
}