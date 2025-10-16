using ProSolution.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ISettingService
    {

        Task UpdateAsync(string id, SettingUpdateDto dto);
       
        Task<ICollection<SettingGetDto>> GetAllAsync();
        Task<PaginationDto<SettingGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
        Task<SettingGetDto> GetByIdAsync(string id);
        Task CreateAsync(SettingCreateDto dto);
    }
}