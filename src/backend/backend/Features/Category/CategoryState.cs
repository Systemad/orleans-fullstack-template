namespace backend.Features.Category;

[GenerateSerializer]
public class CategoryState
{
    [Id(0)]
    public HashSet<string> ProductIds { get; set; } = new();
    [Id(1)]
    public int ItemsAmount { get; set; }
    [Id(2)]
    public string CategoryImage { get; set; }
}