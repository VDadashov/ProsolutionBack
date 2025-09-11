using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities.Identity;
using ProSolution.Core.Repositories;
using System.Security.Claims;
using static ProSolution.BL.DTOs.User.UserAddressDto;

namespace ProSolution.BL.Services.Implements
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _http;

        public UserAddressService(
            IUserAddressRepository repo,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _http = httpContextAccessor;
        }


        public async Task<UserAddressResultDTO> CreateAsync(UserAddressCreateDTO dto)
        {
            string? userId = _http?.HttpContext?.User?.Identity?.IsAuthenticated == true
                         ? _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;

           
            if (userId == null) throw new UnauthorizedAccessException("User not authenticated.");

            var address = _mapper.Map<UserAddress>(dto);
            address.UserId = userId;

            await _repo.AddAsync(address);
            await _repo.SaveChangeAsync();
            return _mapper.Map<UserAddressResultDTO>(address);
        }


        public async Task<UserAddressResultDTO> UpdateAsync(UserAddressUpdateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) throw new NotFoundException<UserAddress>(dto.Id);

            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangeAsync();
            return _mapper.Map<UserAddressResultDTO>(entity);
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException<UserAddress>(id);

            _repo.Delete(entity);
            await _repo.SaveChangeAsync();
        }

        public async Task<UserAddressResultDTO> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException<UserAddress>(id);

            return _mapper.Map<UserAddressResultDTO>(entity);
        }

        public async Task<List<UserAddressResultDTO>> GetAllAsync()
        {
            var list = await _repo.GetAll().ToListAsync();
            return _mapper.Map<List<UserAddressResultDTO>>(list);
        }
    }
}
