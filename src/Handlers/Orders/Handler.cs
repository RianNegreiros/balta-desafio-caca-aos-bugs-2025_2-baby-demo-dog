using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Orders;

public static class Handler
{
    public static async Task<BugStore.Responses.Orders.GetById?> GetByIdAsync(BugStore.Requests.Orders.GetById request, AppDbContext db)
    {
        var order = await db.Orders
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

    public static async Task<BugStore.Responses.Orders.Create> CreateAsync(BugStore.Requests.Orders.Create request, AppDbContext db)
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

        // Validate and compute lines
        foreach (var line in request.Lines)
        {
            var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == line.ProductId);
            if (product is null)
            {
                // Skip invalid product lines; alternatively could throw/return error contract
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

        db.Orders.Add(order);
        await db.SaveChangesAsync();

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


