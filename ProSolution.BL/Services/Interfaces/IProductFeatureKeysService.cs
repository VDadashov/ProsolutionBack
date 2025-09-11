using ProSolution.BL.DTOs.ProductFeatureKeys;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IProductFeatureKeysService
    {
        Task<ProductFeatureKeysGetDto> CreateAsync(ProductFeatureKeysCreateDto dto);
        Task<ProductFeatureKeysGetDto> UpdateAsync(ProductFeatureKeysUpdateDto dto);
        Task DeleteAsync(string id);
        Task<List<ProductFeatureKeysGetDto>> GetAllAsync();
        Task<ProductFeatureKeysGetDto> GetByIdAsync(string id);
    }

}
