using Microsoft.AspNetCore.Http;

namespace ProSolution.BL.DTOs;

public record PartnerUpdateDto
{
    public IFormFile Image { get; set; }
    public string AltText { get; set; }
}
