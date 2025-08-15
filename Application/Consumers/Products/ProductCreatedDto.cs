namespace Application.Consumers.Products;

public class ProductCreatedDto
{
    public Guid Id { get; set; }
    public double Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
