using ProSolution.BL.DTOs.Commons;

namespace ProSolution.BL.DTOs;

public record ProductReviewIncludeDto : ReviewDTO
{
    public string Rating { get; set; } = null!;

    public string ProductId { get; set; }
}
