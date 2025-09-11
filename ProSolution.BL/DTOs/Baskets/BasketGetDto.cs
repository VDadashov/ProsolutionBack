using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BasketGetDto : BaseEntityDTO
{
    public string? Token { get; set; }
    public bool IsVerified { get; set; }

    public ICollection<BasketItemGetDto>? BasketItems { get; set; }
}
