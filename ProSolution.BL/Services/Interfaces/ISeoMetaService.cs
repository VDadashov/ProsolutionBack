using ProSolution.BL.DTOs;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ISeoMetaService
    {
        public Task<IEnumerable<SEOMetaDTO>> GetAllAsync();
        public Task<SEOMetaDTO> GetByIdAsync(string iD);
        public Task<SEOMetaDTO> AddAsync(CreateSEOMetaDTO dto);
        public Task<SEOMetaDTO> UpdateAsync(string id, UpdateSEOMetaDTO dto);
        public Task<SEOMetaDTO> DeleteAsync(string id);
    }
}
