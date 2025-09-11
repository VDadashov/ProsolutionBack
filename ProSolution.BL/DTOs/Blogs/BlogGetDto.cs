using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BlogGetDto : BaseEntityDTO
{
    public string Title { get; set; }
    public string? Slug { get; set; }

    public string Name { get; set; }      
    public string Surname { get; set; }   
    public string ImageUrl { get; set; }
    public string Description { get; set; }

    public string UserId { get; set; }
    public string CategoryId { get; set; }
    public CategoryIncludeDto Category { get; set; }
    public ICollection<BlogReviewIncludeDto>? BlogReviews { get; set; }
}
