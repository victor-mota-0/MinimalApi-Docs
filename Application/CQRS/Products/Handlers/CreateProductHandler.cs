using Application.CQRS.Products.Commands;
using Domain.Entities;
using Domain.Repositories;


namespace Application.CQRS.Products.Handlers;

public class CreateProductHandler(IProductRepository repository)
{
    public async Task HandleAsync(CreateProductCommand command)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Price = command.Price
        };

        await repository.AddAsync(product);
    }
}
