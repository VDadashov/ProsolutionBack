using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record CategoryGetDto : BaseEntityDTO
{
    public string Title { get; set; }
    public string? Slug { get; set; }
    public int? Index { get; set; }

    public int? ProductCount { get; set; }
    public int? ChildCount { get; set; }
    public ICollection<ProductIncludeDto>? Products { get; set; }
    public ICollection<CategoryGetDto>? CategoryItems { get; set; }
}
