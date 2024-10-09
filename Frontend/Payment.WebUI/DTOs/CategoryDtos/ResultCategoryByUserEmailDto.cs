namespace Payment.WebUI.DTOs.CategoryDtos;

public class ResultCategoryByUserEmailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImagePath { get; set; }
    public bool IsActive { get; set; }
    public string CreateUserEmail { get; set; }
    public string UpdateUserEmail { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}
