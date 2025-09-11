using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Categories.CategoryItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryCreateDto dto);

        Task UpdateAsync(string id, CategoryUpdateDto dto);
        Task SoftDeleteAsync(string id, bool isDelete);
        Task DeleteAsync(string id);
        Task<ICollection<CategoryGetDto>> GetAllAsync(string? search, bool isDeleted);
        Task<PaginationDto<CategoryGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted);
        Task<CategoryGetDto> GetByIdAsync(string id);


        Task CreateItemAsync(CategoryItemCreateDto dto);
        Task UpdateItemAsync(string id, CategoryItemUpdateDto dto);
        Task DeleteItemAsync(string id);
    }
}
