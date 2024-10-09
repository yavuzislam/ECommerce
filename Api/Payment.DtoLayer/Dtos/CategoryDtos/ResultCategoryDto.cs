namespace Payment.DtoLayer.Dtos.CategoryDtos;

public class ResultCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImagePath { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}
