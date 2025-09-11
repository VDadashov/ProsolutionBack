using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.DTOs;

public record BrandCreateDto
{
    public IFormFile Image { get; set; }


    public string? Title { get; set; }
    public string? Description { get; set; }
}
