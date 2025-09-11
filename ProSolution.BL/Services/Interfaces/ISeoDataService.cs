using ProSolution.BL.DTOs;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ISeoDataService
    {
        public Task<IEnumerable<SeoDataGetDto>> GetAllAsync();
        public Task<SeoDataGetDto> GetByIdAsync(string id);
        public Task<SeoDataGetDto> AddAsync(CreateSEODTO dto);
        public Task<SeoDataGetDto> UpdateAsync(string id, SeoDataUpdateDto dto);
        public Task<SeoDataGetDto> DeleteAsync(string id);
    }
}
