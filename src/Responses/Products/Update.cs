namespace BugStore.Responses.Products;

public class Update
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public decimal Price { get; set; }
}