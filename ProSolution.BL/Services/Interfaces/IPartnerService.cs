using ProSolution.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IPartnerService
    {
        Task CreateAsync(PartnerCreateDto dto);

        Task UpdateAsync(string id, PartnerUpdateDto dto);
        Task SoftDeleteAsync(string id, bool isDelete);
        Task DeleteAsync(string id);
        Task<ICollection<PartnerGetDto>> GetAllAsync();
        Task<PaginationDto<PartnerGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
        Task<PartnerGetDto> GetByIdAsync(string id);
    }
}

