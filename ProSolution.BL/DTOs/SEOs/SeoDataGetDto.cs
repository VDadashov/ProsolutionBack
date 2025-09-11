using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record SeoDataGetDto : BaseEntityDTO
{
    public string? AltText { get; set; }
    public string? AnchorText { get; set; }
}
