using BugStore.Services.Orders;
using Microsoft.AspNetCore.Routing;

namespace BugStore.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/orders/{id}", async (IOrderService service, Guid id) =>
        {
            var response = await service.GetByIdAsync(new BugStore.Requests.Orders.GetById { Id = id });
            return response is null ? Results.NotFound() : Results.Ok(response);
        });

        app.MapPost("/v1/orders", async (IOrderService service, BugStore.Requests.Orders.Create request) =>
        {
            var response = await service.CreateAsync(request);
            return Results.Created($"/v1/orders/{response.Id}", response);
        });

        return app;
    }
}


