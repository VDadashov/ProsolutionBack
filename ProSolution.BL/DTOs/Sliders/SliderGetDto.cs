using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record SliderGetDto : BaseEntityDTO
{
    public string ImagePath { get; set; }
    public string AltText { get; set; }
}
