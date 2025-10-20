﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repository;
        private readonly IMapper _mapper;

        public SettingService(ISettingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginationDto<SettingGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted = false)
        {
            if (page <= 0 || take <= 0 || order <= 0 || order > 4)
                throw new NegativIdException("Filter parametrləri düzgün deyil");

            double count = await _repository.CountAsync(
                x => string.IsNullOrEmpty(search) || x.Key.ToLower().Contains(search.ToLower()),
                isDeleted
            );

            ICollection<Setting> items = order switch
            {
                1 => await _repository.GetAllWhereByOrder(
                            x => string.IsNullOrEmpty(search) || x.Key.ToLower().Contains(search.ToLower()),
                            x => x.Key, false, isDeleted, (page - 1) * take, take, false).ToListAsync(),

                2 => await _repository.GetAllWhereByOrder(
                            x => string.IsNullOrEmpty(search) || x.Key.ToLower().Contains(search.ToLower()),
                            x => x.CreatedAt, false, isDeleted, (page - 1) * take, take, false).ToListAsync(),

                3 => await _repository.GetAllWhereByOrder(
                            x => string.IsNullOrEmpty(search) || x.Key.ToLower().Contains(search.ToLower()),
                            x => x.Key, true, isDeleted, (page - 1) * take, take, false).ToListAsync(),

                4 => await _repository.GetAllWhereByOrder(
                            x => string.IsNullOrEmpty(search) || x.Key.ToLower().Contains(search.ToLower()),
                            x => x.CreatedAt, true, isDeleted, (page - 1) * take, take, false).ToListAsync(),

                _ => throw new NegativIdException("Siralama parametri yanlisdir")
            };

            var dtos = _mapper.Map<ICollection<SettingGetDto>>(items);

            return new PaginationDto<SettingGetDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<ICollection<SettingGetDto>> GetAllAsync()
        {
            var settings = await _repository.GetAll().ToListAsync();
            return _mapper.Map<ICollection<SettingGetDto>>(settings);
        }

        public async Task<SettingGetDto> GetByIdAsync(string id)
        {
            var setting = await _getByIdAsync(id, true);
            return _mapper.Map<SettingGetDto>(setting);
        }

        public async Task UpdateAsync(string id, SettingUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException("ID bos ola bilmez");

            var setting = await _getByIdAsync(id);

            if (await _repository.CheckUniqueAsync(x => x.Key.ToLower() == dto.Value.ToLower().Trim() && x.Id != id))
                throw new NegativIdException($"{dto.Value} artıq mövcuddur!");

            _mapper.Map(dto, setting);
            _repository.Update(setting);
            await _repository.SaveChangeAsync();
        }

        private async Task<Setting> _getByIdAsync(string id, bool isTracking = true, params string[] includes)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException("ID qeyd olunmayib");

            var setting = await _repository.GetByIdAsync(id, isTracking, includes);
            if (setting is null)
                throw new NotFoundException<Setting>();

            return setting;
        }
        
        public async Task CreateAsync(SettingCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Key))
                throw new ArgumentException("Key can not be null");

            if (string.IsNullOrWhiteSpace(dto.Value))
                throw new ArgumentException("Value can not be null");

            if (await _repository.CheckUniqueAsync(x => x.Key.ToLower() == dto.Key.ToLower().Trim()))
                throw new Exception($"{dto.Key} already exists!");

            var setting = _mapper.Map<Setting>(dto);

            await _repository.AddAsync(setting);
            await _repository.SaveChangeAsync();
        }

    }
}