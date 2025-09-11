using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BlogReviewReplyGetDto : ReviewDTO
{
  

    public string BlogReviewId { get; set; }
    public BlogReviewIncludeDto BlogReview { get; set; }
}
