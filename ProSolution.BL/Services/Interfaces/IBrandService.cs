using ProSolution.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IBrandService
    {
        Task CreateAsync(BrandCreateDto dto);

        Task UpdateAsync(string id, BrandUpdateDto dto);
        Task SoftDeleteAsync(string id, bool isDelete);
        Task DeleteAsync(string id);
        Task<ICollection<BrandGetDto>> GetAllAsync();
        Task<PaginationDto<BrandGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
        Task<BrandGetDto> GetByIdAsync(string id);
    }
}
