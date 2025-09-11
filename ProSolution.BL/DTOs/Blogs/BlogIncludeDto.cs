using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BlogIncludeDto: BaseEntityDTO
{
    // public string UserImageUrl { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }

    public string Surname { get; set; }
    public string? UserId { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}
