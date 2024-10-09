namespace Payment.WebUI.DTOs.ProductDtos;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile ImageFile { get; set; }
    public string? ImagePath { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountRate { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
}
