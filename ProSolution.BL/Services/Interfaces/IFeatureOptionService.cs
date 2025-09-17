using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.FeatureOPtions;

namespace ProSolution.BL.Services.Interfaces;

public interface IFeatureOptionService
{
    Task<ICollection<FeatureOptionGetDto>> GetAll();


    Task<PaginationDto<FeatureOptionGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);

    Task<FeatureOptionGetDto?> GetByIdAsync(string id);

    Task CreateAsync(FeatureOptionCreateDto dto);
    // Task UpdateAsync(FeatureOptionUpdateDto dto);
    Task UpdateAsync(string id, FeatureOptionUpdateDto dto);
    Task DeleteAsync(string id);

    // Для Item'ов
    Task CreateItemAsync(FeatureOptionItemCreateDto dto);
    Task UpdateItemAsync(FeatureOptionItemUpdateDto dto);
    Task DeleteItemAsync(string id);
}
