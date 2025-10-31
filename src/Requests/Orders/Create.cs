namespace BugStore.Requests.Orders;

public class Create
{
    public Guid CustomerId { get; set; }
    public List<Line> Lines { get; set; } = [];

    public class Line
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}