using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BlogReviewReplyUpdateDto
{
    public string Text { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
  
    public string BlogReviewId { get; set; }
}
