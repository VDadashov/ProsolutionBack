using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProSolution.BL.DTOs.User.UserAddressDto;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IUserAddressService
    {
        Task<UserAddressResultDTO> CreateAsync(UserAddressCreateDTO dto);
        Task<UserAddressResultDTO> UpdateAsync(UserAddressUpdateDTO dto);
        Task DeleteAsync(string id);
        Task<UserAddressResultDTO> GetByIdAsync(string id);
        Task<List<UserAddressResultDTO>> GetAllAsync();
    }
}
