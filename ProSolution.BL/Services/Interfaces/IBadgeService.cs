using ProSolution.BL.DTOs;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IBadgeService
    {
        Task CreateAsync(BadgeCreateDto dto);

        Task UpdateAsync(string id, BadgeUpdateDto dto);
        Task SoftDeleteAsync(string id, bool isDelete);
        Task DeleteAsync(string id);
        Task<ICollection<BadgeGetDto>> GetAllAsync();
        Task<PaginationDto<BadgeGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
        Task<BadgeGetDto> GetByIdAsync(string id);
    }
}
