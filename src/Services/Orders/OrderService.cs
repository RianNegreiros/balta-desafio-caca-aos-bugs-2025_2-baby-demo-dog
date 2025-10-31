using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Services.Orders;

public class OrderService(AppDbContext db) : IOrderService
{
    private readonly AppDbContext _db = db;

    public async Task<BugStore.Responses.Orders.GetById?> GetByIdAsync(BugStore.Requests.Orders.GetById request)
    {
        var order = await _db.Orders
            .AsNoTracking()
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == request.Id);

        if (order is null) return null;

        return new BugStore.Responses.Orders.GetById
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Lines = order.Lines.Select(l => new BugStore.Responses.Orders.GetById.Line
            {
                Id = l.Id,
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                Total = l.Total
            }).ToList()
        };
    }

    public async Task<BugStore.Responses.Orders.Create> CreateAsync(BugStore.Requests.Orders.Create request)
    {
        var now = DateTime.UtcNow;
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedAt = now,
            UpdatedAt = now,
            Lines = new List<OrderLine>()
        };

        foreach (var line in request.Lines)
        {
            var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == line.ProductId);
            if (product is null)
            {
                continue;
            }

            var total = product.Price * line.Quantity;
            order.Lines.Add(new OrderLine
            {
                Id = Guid.NewGuid(),
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                Total = total
            });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return new BugStore.Responses.Orders.Create
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Lines = order.Lines.Select(l => new BugStore.Responses.Orders.Create.Line
            {
                Id = l.Id,
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                Total = l.Total
            }).ToList()
        };
    }
}


