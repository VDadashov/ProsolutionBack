using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BadgeGetDto : BaseEntityDTO
{
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public bool IsSertificate { get; set; }

}
