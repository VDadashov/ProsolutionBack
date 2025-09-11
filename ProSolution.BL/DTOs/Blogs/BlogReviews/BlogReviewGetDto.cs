using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record BlogReviewGetDto : ReviewDTO
{
  
    public string BlogId { get; set; }
    public BlogIncludeDto Blog { get; set; }

    public ICollection<BlogReviewReplyIncludeDto>? BlogReviewReplies { get; set; }
}
    