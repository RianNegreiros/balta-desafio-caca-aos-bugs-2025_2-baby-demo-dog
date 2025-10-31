namespace BugStore.Responses.Products;

public class Get
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<Item> Items { get; set; } = [];

    public class Item
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public decimal Price { get; set; }
    }
}