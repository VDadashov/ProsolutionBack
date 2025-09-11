using Microsoft.AspNetCore.Http;
using ProSolution.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ISliderService
    {
        Task CreateAsync(SliderCreateDto dto);
      
        Task UpdateAsync(string id, SliderUpdateDto dto);
        Task SoftDeleteAsync(string id, bool isDelete);
        Task DeleteAsync(string id);
        Task<ICollection<SliderGetDto>> GetAllAsync();
        Task<PaginationDto<SliderGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
        Task<SliderGetDto> GetByIdAsync(string id);
    }
}
