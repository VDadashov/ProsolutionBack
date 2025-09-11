using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record ProductSlugGetDto : BaseEntityDTO
{
    public string? Slug { get; set; }

    public string? ProductId { get; set; }
}
