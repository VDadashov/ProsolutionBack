using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BasketItemGetDto : BaseEntityDTO
{
    public int Count { get; set; }

    public string? ProductId { get; set; } 
    public ProductIncludeDto? Product { get; set; }
}
