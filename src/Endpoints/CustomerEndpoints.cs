using BugStore.Services.Customers;
using Microsoft.AspNetCore.Routing;

namespace BugStore.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/customers", async (ICustomerService service, BugStore.Requests.Customers.Get request) =>
        {
            var response = await service.GetAsync(request);
            return Results.Ok(response);
        });

        app.MapGet("/v1/customers/{id}", async (ICustomerService service, Guid id) =>
        {
            var response = await service.GetByIdAsync(new BugStore.Requests.Customers.GetById { Id = id });
            return response is null ? Results.NotFound() : Results.Ok(response);
        });

        app.MapPost("/v1/customers", async (ICustomerService service, BugStore.Requests.Customers.Create request) =>
        {
            var response = await service.CreateAsync(request);
            return Results.Created($"/v1/customers/{response.Id}", response);
        });

        app.MapPut("/v1/customers/{id}", async (ICustomerService service, Guid id, BugStore.Requests.Customers.Update request) =>
        {
            var response = await service.UpdateAsync(id, request);
            return response is null ? Results.NotFound() : Results.Ok(response);
        });

        app.MapDelete("/v1/customers/{id}", async (ICustomerService service, Guid id) =>
        {
            var response = await service.DeleteAsync(new BugStore.Requests.Customers.Delete { Id = id });
            return response.Deleted ? Results.Ok(response) : Results.NotFound(response);
        });

        return app;
    }
}


