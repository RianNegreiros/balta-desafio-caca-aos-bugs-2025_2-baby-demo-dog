namespace BugStore.Requests.Products;

public class Create
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public decimal Price { get; set; }
}