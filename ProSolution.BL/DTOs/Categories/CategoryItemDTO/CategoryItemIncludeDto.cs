using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs.Categories.CategoryItemDTO;

public record CategoryItemIncludeDto : BaseEntityDTO
{
    public string Title { get; set; }
    public string? Slug { get; set; }
    public int? Index { get; set; }
}
