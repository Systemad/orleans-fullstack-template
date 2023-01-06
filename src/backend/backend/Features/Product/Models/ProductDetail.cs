namespace backend.Features.Product.Models;

[GenerateSerializer]
public record ProductDetail
{
    [Id(0)]
    public string Id { get; set; } = null!;
    [Id(1)]
    public string Name { get; set; } = null!;
    [Id(2)]
    public string Description { get; set; } = null!;
    [Id(3)]
    public ProductCategory Category { get; set; }
    [Id(4)]
    public int Quantity { get; set; }
    [Id(5)]
    public decimal Price { get; set; }
    [Id(6)]
    public string ImageUrl { get; set; } = null!;
}