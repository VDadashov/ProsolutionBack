using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.DTOs;

public record BadgeCreateDto
{
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }

    public bool IsSertificate { get; set; }

}
