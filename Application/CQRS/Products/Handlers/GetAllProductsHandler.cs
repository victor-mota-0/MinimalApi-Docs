using Application.Common;
using Application.CQRS.Products.Queries;
using Domain.Entities;
using Domain.Repositories;

namespace Application.CQRS.Products.Handlers;

public class GetAllProductsHandler(IProductRepository repository)
{
    private readonly IProductRepository _repository = repository;

    public async Task<Result<IEnumerable<Product>>> HandleAsync(GetAllProductsQuery query)
    {
        var products = await _repository.GetAllAsync();
        return Result<IEnumerable<Product>>.Success(products);
    }
}
