using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BrandGetDto : BaseEntityDTO
{
    public string? ImagePath { get; set; } // вместо IFormFile

    public string? Title { get; set; }
    public string? Slug { get; set; }

    public string? Description { get; set; }
    public int? ProductCount { get; set; }

    public ICollection<ProductIncludeDto>? Products { get; set; }
}
