namespace Application.CQRS.Products.Commands;

public class CreateProductCommand
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}