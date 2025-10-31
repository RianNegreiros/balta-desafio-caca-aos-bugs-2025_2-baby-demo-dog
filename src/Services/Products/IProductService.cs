namespace BugStore.Services.Products;

public interface IProductService
{
    Task<BugStore.Responses.Products.Get> GetAsync(BugStore.Requests.Products.Get request);
    Task<BugStore.Responses.Products.GetById?> GetByIdAsync(BugStore.Requests.Products.GetById request);
    Task<BugStore.Responses.Products.Create> CreateAsync(BugStore.Requests.Products.Create request);
    Task<BugStore.Responses.Products.Update?> UpdateAsync(Guid id, BugStore.Requests.Products.Update request);
    Task<BugStore.Responses.Products.Delete> DeleteAsync(BugStore.Requests.Products.Delete request);
}


