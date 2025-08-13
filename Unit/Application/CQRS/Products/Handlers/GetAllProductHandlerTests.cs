using Application.CQRS.Products.Handlers;
using Application.CQRS.Products.Queries;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace Unit.Application.CQRS.Products.Handlers;

public class GetAllProductsHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Produto 1", Price = 10 },
            new() { Id = Guid.NewGuid(), Name = "Produto 2", Price = 20 }
        };

        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var handler = new GetAllProductsHandler(repoMock.Object);
        var query = new GetAllProductsQuery();

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, ((ICollection<Product>)result.Value).Count);
        repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}