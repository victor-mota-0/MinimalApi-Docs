using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace Application.Consumers.Products;


public class ProductConsumer(ILogger<ProductConsumer> logger) : ICapSubscribe
{
    [CapSubscribe("product.created")]
    public void ProductCreatedEvent(ProductCreatedDto dto)
        => logger.LogInformation($"Product created: {dto.Id} - ${dto.Price} - {dto.CreatedAt}");
}
