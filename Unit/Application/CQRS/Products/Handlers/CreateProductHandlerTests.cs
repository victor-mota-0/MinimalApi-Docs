using Application.CQRS.Products.Commands;
using Application.CQRS.Products.Handlers;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace Unit.Application.CQRS.Products.Handlers;

public class CreateProductHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidCommand_ReturnsSuccessResultAndCallsRepository()
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();
        var handler = new CreateProductHandler(repoMock.Object);
        var command = new CreateProductCommand { Name = "Produto Teste", Price = 10.5m };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Name, result.Value.Name);
        Assert.Equal(command.Price, result.Value.Price);
        repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task HandleAsync_WithInvalidName_ReturnsFailureResult(string? invalidName)
    {
        // Arrange
        var repoMock = new Mock<IProductRepository>();
        var handler = new CreateProductHandler(repoMock.Object);
        var command = new CreateProductCommand { Name = invalidName, Price = 10.5m };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Nome do produto é obrigatório.", result.Error);
        repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }
}