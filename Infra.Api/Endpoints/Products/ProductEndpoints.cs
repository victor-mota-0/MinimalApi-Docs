using Application.Consumers.Products;
using Application.CQRS.Products.Commands;
using Application.CQRS.Products.Handlers;
using DotNetCore.CAP;

namespace Infra.Api.Endpoints.Products;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductCommand command, CreateProductHandler handler) =>
        {
            var result = await handler.HandleAsync(command);
            if (!result.IsSuccess)
                return Results.BadRequest(new { error = result.Error });

            return Results.Created($"/products/{result.Value!.Id}", result.Value);
        });

        app.MapGet("/products", async (GetAllProductsHandler handler) =>
        {
            var result = await handler.HandleAsync(new());
            if (!result.IsSuccess)
                return Results.BadRequest(new { error = result.Error });

            return Results.Ok(result.Value);
        });

        app.MapPost("/products/event", (ICapPublisher capBus) =>
        {
            capBus.Publish("product.created", new ProductCreatedDto
            {
                Id = Guid.NewGuid(),
                Price = 100.0,
                CreatedAt = DateTime.UtcNow
            });

            return Results.Ok(new { Message = "Event published" });
        });
    }
}
