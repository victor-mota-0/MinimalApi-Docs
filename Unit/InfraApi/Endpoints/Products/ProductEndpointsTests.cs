using Application.Common;
using Application.CQRS.Products.Handlers;
using Application.CQRS.Products.Queries;
using Domain.Entities;
using Moq;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.Repositories;

namespace Unit.InfraApi.Endpoints.Products;

public class ProductEndpointsTests
{
    private class FakeGetAllProductsHandler(Func<GetAllProductsQuery, Task<Result<IEnumerable<Product>>>> handleAsync) : GetAllProductsHandler(new Mock<IProductRepository>().Object)
    {
        private readonly Func<GetAllProductsQuery, Task<Result<IEnumerable<Product>>>> _handleAsync = handleAsync;

        public new Task<Result<IEnumerable<Product>>> HandleAsync(GetAllProductsQuery query)
            => _handleAsync(query);
    }

    [Fact]
    public async Task GetProducts_Failure_ReturnsBadRequest()
    {
        var handler = new FakeGetAllProductsHandler(_ => Task.FromResult(Result<IEnumerable<Product>>.Failure("Erro ao buscar produtos.")));

        Task<IResult> endpoint(FakeGetAllProductsHandler h) => h.HandleAsync(new()).ContinueWith(t =>
            t.Result.IsSuccess
                ? Results.Ok(t.Result.Value)
                : Results.BadRequest(new { error = t.Result.Error })
        );

        var result = await endpoint(handler);
        dynamic badRequest = result;
        Assert.Equal("Erro ao buscar produtos.", badRequest.Value.error);
    }

    [Fact]
    public async Task GetProducts_Success_ReturnsOkWithProducts()
    {
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Produto 1", Price = 10 },
            new Product { Id = Guid.NewGuid(), Name = "Produto 2", Price = 20 }
        };

        var handler = new FakeGetAllProductsHandler(_ => Task.FromResult(Result<IEnumerable<Product>>.Success(products)));

        Task<IResult> endpoint(FakeGetAllProductsHandler h) => h.HandleAsync(new()).ContinueWith(t =>
            t.Result.IsSuccess
                ? Results.Ok(t.Result.Value)
                : Results.BadRequest(new { error = t.Result.Error })
        );

        var result = await endpoint(handler);
        var okResult = Assert.IsType<Ok<IEnumerable<Product>>>(result);
        Assert.Equal(2, okResult.Value.Count());
        Assert.Contains(okResult.Value, p => p.Name == "Produto 1");
        Assert.Contains(okResult.Value, p => p.Name == "Produto 2");
    }

    [Fact]
    public async Task GetProducts_Success_ReturnsOkWithEmptyList()
    {
        var handler = new FakeGetAllProductsHandler(_ => Task.FromResult(Result<IEnumerable<Product>>.Success([])));

        Task<IResult> endpoint(FakeGetAllProductsHandler h) => h.HandleAsync(new()).ContinueWith(t =>
            t.Result.IsSuccess
                ? Results.Ok(t.Result.Value)
                : Results.BadRequest(new { error = t.Result.Error })
        );

        var result = await endpoint(handler);
        var okResult = Assert.IsType<Ok<IEnumerable<Product>>>(result);
        Assert.Empty(okResult.Value);
    }

    [Fact]
    public async Task GetProducts_Exception_ReturnsBadRequest()
    {
        var handler = new FakeGetAllProductsHandler(_ => throw new Exception("Exceção inesperada"));

        Task<IResult> endpoint(FakeGetAllProductsHandler h)
        {
            try
            {
                return h.HandleAsync(new()).ContinueWith(t =>
                    t.Result.IsSuccess
                        ? Results.Ok(t.Result.Value)
                        : Results.BadRequest(new { error = t.Result.Error })
                );
            }
            catch (Exception ex)
            {
                return Task.FromResult<IResult>(Results.BadRequest(new { error = ex.Message }));
            }
        }

        var result = await endpoint(handler);
        dynamic badRequest = result;
        Assert.Equal("Exceção inesperada", badRequest.Value.error);
    }
}