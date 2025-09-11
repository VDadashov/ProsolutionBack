using ProSolution.BL.DTOs;

namespace ProSolution.BL.Services.Interfaces;

public interface IBasketService
{
    Task<string> CreateAsync(BasketCreateDto dto);
    Task<string> UpdateAsync(string id, BasketUpdateDto dto);
    Task<string> SoftDeleteAsync(string id);
    Task<string> DeleteAsync(string id);
    Task<ICollection<BasketGetDto>> GetAllAsync();
    Task<PaginationDto<BasketGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
    Task<BasketGetDto> GetByIdAsync(string id);
    Task<BasketGetDto> GetByTokenAsync(string token);

  

}
