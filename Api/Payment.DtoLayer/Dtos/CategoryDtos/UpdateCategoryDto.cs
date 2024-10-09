﻿using Microsoft.AspNetCore.Http;

namespace Payment.DtoLayer.Dtos.CategoryDtos;

public class UpdateCategoryDto
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? ImagePath { get; set; }
}
