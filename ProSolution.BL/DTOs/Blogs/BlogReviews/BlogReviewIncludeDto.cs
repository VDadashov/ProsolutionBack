using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BlogReviewIncludeDto : ReviewDTO
{
  

    public string BlogId { get; set; }

    public ICollection<BlogReviewReplyIncludeDto>? BlogReviewReplies { get; set; }
}
