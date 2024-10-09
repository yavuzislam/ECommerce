namespace Payment.WebUI.DTOs.CategoryDtos;

public class CreateCategoryDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile ImageFile { get; set; }
    public string? ImagePath { get; set; }
}
