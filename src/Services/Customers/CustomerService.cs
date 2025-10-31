using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Services.Customers;

public class CustomerService(AppDbContext db) : ICustomerService
{
    private readonly AppDbContext _db = db;

    public async Task<BugStore.Responses.Customers.Get> GetAsync(BugStore.Requests.Customers.Get request)
    {
        var queryable = _db.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            var q = request.Query.ToLower();
            queryable = queryable.Where(c =>
                c.Name.ToLower().Contains(q) ||
                c.Email.ToLower().Contains(q) ||
                c.Phone.ToLower().Contains(q));
        }

        var total = await queryable.CountAsync();
        var skip = (request.Page - 1) * request.PageSize;
        var customers = await queryable
            .OrderBy(c => c.Name)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync();

        return new BugStore.Responses.Customers.Get
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Total = total,
            Items = customers.Select(c => new BugStore.Responses.Customers.Get.Item
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                BirthDate = c.BirthDate
            }).ToList()
        };
    }

    public async Task<BugStore.Responses.Customers.GetById?> GetByIdAsync(BugStore.Requests.Customers.GetById request)
    {
        var c = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);
        if (c is null) return null;
        return new BugStore.Responses.Customers.GetById
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            BirthDate = c.BirthDate
        };
    }

    public async Task<BugStore.Responses.Customers.Create> CreateAsync(BugStore.Requests.Customers.Create request)
    {
        var entity = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            BirthDate = request.BirthDate
        };

        _db.Customers.Add(entity);
        await _db.SaveChangesAsync();

        return new BugStore.Responses.Customers.Create
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Phone = entity.Phone,
            BirthDate = entity.BirthDate
        };
    }

    public async Task<BugStore.Responses.Customers.Update?> UpdateAsync(Guid id, BugStore.Requests.Customers.Update request)
    {
        var entity = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return null;

        entity.Name = request.Name;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.BirthDate = request.BirthDate;

        await _db.SaveChangesAsync();

        return new BugStore.Responses.Customers.Update
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Phone = entity.Phone,
            BirthDate = entity.BirthDate
        };
    }

    public async Task<BugStore.Responses.Customers.Delete> DeleteAsync(BugStore.Requests.Customers.Delete request)
    {
        var entity = await _db.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (entity is null)
        {
            return new BugStore.Responses.Customers.Delete { Id = request.Id, Deleted = false };
        }

        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync();

        return new BugStore.Responses.Customers.Delete { Id = request.Id, Deleted = true };
    }
}


