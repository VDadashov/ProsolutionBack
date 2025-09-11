using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.DTOs;

public record PartnerCreateDto
{
    public IFormFile Image { get; set; }
    public string? AltText { get; set; }
}
