namespace BugStore.Services.Orders;

public interface IOrderService
{
    Task<BugStore.Responses.Orders.GetById?> GetByIdAsync(BugStore.Requests.Orders.GetById request);
    Task<BugStore.Responses.Orders.Create> CreateAsync(BugStore.Requests.Orders.Create request);
}


