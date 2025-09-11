using ProSolution.BL.DTOs;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IBlogService
    {
        // Blog
        Task CreateAsync(BlogCreateDto dto);
        Task UpdateAsync(string id, BlogUpdateDto dto);
        Task SoftDeleteAsync(string id, bool isDelete);
        Task DeleteAsync(string id);
        Task<ICollection<BlogGetDto>> GetAllAsync();
        Task<PaginationDto<BlogGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted,string? authorId);
        Task<BlogGetDto> GetByIdAsync(string id);
        Task<BlogGetDto> GetBySlugAsync(string slug);

        // BlogReview
        Task<BlogReviewGetDto> AddReviewAsync(BlogReviewCreateDto dto);
     
        Task DeleteReviewAsync(string id);

        // BlogReviewReply
        Task<BlogReviewReplyGetDto> AddReviewReplyAsync(BlogReviewReplyCreateDto dto);
      
        Task DeleteReviewReplyAsync(string id);
    }
}
