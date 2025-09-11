using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BlogReviewUpdateDto
{
    public string Text { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
   
    public string BlogId { get; set; }
}
