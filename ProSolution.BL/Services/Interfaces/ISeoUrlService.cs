using ProSolution.BL.DTOs;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ISeoUrlService
    {
        public Task<IEnumerable<SEOUrlDTO>> GetAllAsync();
        public Task<SEOUrlDTO> GetByIdAsync(string id);
        public Task<SEOUrlDTO> AddAsync(CreateSeoUrlDTO dto);
        public Task<SEOUrlDTO> UpdateAsync(string id,UpdateSEOUrlDTO dto);
        public Task<SEOUrlDTO> DeleteAsync(string id);
    }
}
