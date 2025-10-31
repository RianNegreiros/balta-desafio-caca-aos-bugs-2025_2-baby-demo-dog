using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Products;

public static class Handler
{
    public static async Task<BugStore.Responses.Products.Get> GetAsync(BugStore.Requests.Products.Get request, AppDbContext db)
    {
        var queryable = db.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            var q = request.Query.ToLower();
            queryable = queryable.Where(p =>
                p.Title.ToLower().Contains(q) ||
                p.Description.ToLower().Contains(q) ||
                p.Slug.ToLower().Contains(q));
        }

        var total = await queryable.CountAsync();
        var skip = (request.Page - 1) * request.PageSize;
        var products = await queryable
            .OrderBy(p => p.Title)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync();

        return new BugStore.Responses.Products.Get
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Total = total,
            Items = products.Select(p => new BugStore.Responses.Products.Get.Item
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Slug = p.Slug,
                Price = p.Price
            }).ToList()
        };
    }

    public static async Task<BugStore.Responses.Products.GetById?> GetByIdAsync(BugStore.Requests.Products.GetById request, AppDbContext db)
    {
        var p = await db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);
        if (p is null) return null;
        return new BugStore.Responses.Products.GetById
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Slug = p.Slug,
            Price = p.Price
        };
    }

    public static async Task<BugStore.Responses.Products.Create> CreateAsync(BugStore.Requests.Products.Create request, AppDbContext db)
    {
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Slug = request.Slug,
            Price = request.Price
        };

        db.Products.Add(entity);
        await db.SaveChangesAsync();

        return new BugStore.Responses.Products.Create
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Slug = entity.Slug,
            Price = entity.Price
        };
    }

    public static async Task<BugStore.Responses.Products.Update?> UpdateAsync(Guid id, BugStore.Requests.Products.Update request, AppDbContext db)
    {
        var entity = await db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return null;

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Slug = request.Slug;
        entity.Price = request.Price;

        await db.SaveChangesAsync();

        return new BugStore.Responses.Products.Update
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Slug = entity.Slug,
            Price = entity.Price
        };
    }

    public static async Task<BugStore.Responses.Products.Delete> DeleteAsync(BugStore.Requests.Products.Delete request, AppDbContext db)
    {
        var entity = await db.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (entity is null)
        {
            return new BugStore.Responses.Products.Delete { Id = request.Id, Deleted = false };
        }

        db.Products.Remove(entity);
        await db.SaveChangesAsync();

        return new BugStore.Responses.Products.Delete { Id = request.Id, Deleted = true };
    }
}


