using BugStore.Data;

namespace BugStore.Services.Customers;

public interface ICustomerService
{
    Task<BugStore.Responses.Customers.Get> GetAsync(BugStore.Requests.Customers.Get request);
    Task<BugStore.Responses.Customers.GetById?> GetByIdAsync(BugStore.Requests.Customers.GetById request);
    Task<BugStore.Responses.Customers.Create> CreateAsync(BugStore.Requests.Customers.Create request);
    Task<BugStore.Responses.Customers.Update?> UpdateAsync(Guid id, BugStore.Requests.Customers.Update request);
    Task<BugStore.Responses.Customers.Delete> DeleteAsync(BugStore.Requests.Customers.Delete request);
}


