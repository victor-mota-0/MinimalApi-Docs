using Application.CQRS.Products.Commands;
using Application.CQRS.Products.Handlers;

namespace Infra.Api.Endpoints.Products;

public static class ProductEndpoint
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductCommand command, CreateProductHandler handler) =>
        {
            await handler.HandleAsync(command);
            return Results.Created($"/products", command);
        });

        app.MapGet("/products", async (GetAllProductsHandler handler) =>
        {
            var products = await handler.HandleAsync(new());
            return Results.Ok(products);
        });
    }
}
