using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record CategoryIncludeDto : BaseEntityDTO 
{
    public string Title { get; set; }
    public string? Slug { get; set; }
    public int? Index { get; set; }
}
