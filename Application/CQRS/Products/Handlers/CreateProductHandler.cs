using Application.Common;
using Application.CQRS.Products.Commands;
using Domain.Entities;
using Domain.Repositories;


namespace Application.CQRS.Products.Handlers;

public class CreateProductHandler(IProductRepository repository)
{
    public async Task<Result<Product>> HandleAsync(CreateProductCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<Product>.Failure("Nome do produto é obrigatório.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Price = command.Price
        };

        await repository.AddAsync(product);
        return Result<Product>.Success(product);
    }
}
