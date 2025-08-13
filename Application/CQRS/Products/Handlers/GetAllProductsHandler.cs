using Application.CQRS.Products.Queries;
using Domain.Entities;
using Domain.Repositories;

namespace Application.CQRS.Products.Handlers;

public class GetAllProductsHandler(IProductRepository repository)
{
    private readonly IProductRepository _repository = repository;

    public async Task<IEnumerable<Product>> HandleAsync(GetAllProductsQuery query)
    {
        return await _repository.GetAllAsync();
    }
}
