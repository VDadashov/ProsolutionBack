using ProSolution.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IOurServiceService
    {
        Task CreateAsync(OurServiceCreateDto dto);

        Task UpdateAsync(string id, OurServiceUpdateDto dto);
        Task SoftDeleteAsync(string id, bool isDelete);
        Task DeleteAsync(string id);
        Task<ICollection<OurServiceGetDto>> GetAllAsync();
        Task<PaginationDto<OurServiceGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
        Task<OurServiceGetDto> GetByIdAsync(string id);
    }
}
