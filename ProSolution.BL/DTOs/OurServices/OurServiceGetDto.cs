using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record OurServiceGetDto : BaseEntityDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }

    public string ContentTitle { get; set; }
    public string ContentDescription { get; set; }

    public string ContentPath { get; set; }
}
