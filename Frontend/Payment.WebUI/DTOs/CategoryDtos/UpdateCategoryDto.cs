namespace Payment.WebUI.DTOs.CategoryDtos;

public class UpdateCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? ImagePath { get; set; }
}
