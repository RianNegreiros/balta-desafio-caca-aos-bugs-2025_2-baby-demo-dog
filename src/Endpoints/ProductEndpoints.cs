using BugStore.Services.Products;
using Microsoft.AspNetCore.Routing;

namespace BugStore.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/products", async (IProductService service, BugStore.Requests.Products.Get request) =>
        {
            var response = await service.GetAsync(request);
            return Results.Ok(response);
        });

        app.MapGet("/v1/products/{id}", async (IProductService service, Guid id) =>
        {
            var response = await service.GetByIdAsync(new BugStore.Requests.Products.GetById { Id = id });
            return response is null ? Results.NotFound() : Results.Ok(response);
        });

        app.MapPost("/v1/products", async (IProductService service, BugStore.Requests.Products.Create request) =>
        {
            var response = await service.CreateAsync(request);
            return Results.Created($"/v1/products/{response.Id}", response);
        });

        app.MapPut("/v1/products/{id}", async (IProductService service, Guid id, BugStore.Requests.Products.Update request) =>
        {
            var response = await service.UpdateAsync(id, request);
            return response is null ? Results.NotFound() : Results.Ok(response);
        });

        app.MapDelete("/v1/products/{id}", async (IProductService service, Guid id) =>
        {
            var response = await service.DeleteAsync(new BugStore.Requests.Products.Delete { Id = id });
            return response.Deleted ? Results.Ok(response) : Results.NotFound(response);
        });

        return app;
    }
}


