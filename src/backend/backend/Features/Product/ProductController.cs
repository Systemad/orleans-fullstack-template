using Microsoft.AspNetCore.Mvc;

namespace backend.Features.Product;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;
    // GET
    public ProductController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public IActionResult Index()
    {
        //return View();
        return Ok();
    }
}