using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements
{
    public class SeoDataService : ISeoDataService
    {
        private readonly ISeoDataRepository _seoRepository;
        private readonly IMapper _mapper;

        public SeoDataService(ISeoDataRepository seoRepository, IMapper mapper)
        {
            _seoRepository = seoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeoDataGetDto>> GetAllAsync()
        {
            var entities = await _seoRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<SeoDataGetDto>>(entities);
        }

        public async Task<SeoDataGetDto> GetByIdAsync(string id)
        {
            var entity = await _seoRepository.GetByIdAsync(id, false);
            if (entity == null)
            {
                throw new Exception($"SEO entry with ID {id} not found."); 
            }

            return _mapper.Map<SeoDataGetDto>(entity);
        }

        public async Task<SeoDataGetDto> AddAsync(CreateSEODTO dto)
        {
            var entity = _mapper.Map<SeoData>(dto);
            await _seoRepository.AddAsync(entity);
            await _seoRepository.SaveChangeAsync();

            return _mapper.Map<SeoDataGetDto>(entity);
        }

        public async Task<SeoDataGetDto> UpdateAsync(string id, SeoDataUpdateDto dto)
        {
            var entity = await _seoRepository.GetByIdAsync(id, false);
            if (entity == null)
            {
                throw new Exception($"SEO entry with ID {id} not found.");
            }

            _mapper.Map(dto, entity);
            _seoRepository.Update(entity);
            await _seoRepository.SaveChangeAsync();

            return _mapper.Map<SeoDataGetDto>(entity);
        }

        public async Task<SeoDataGetDto> DeleteAsync(string id)
        {
            var entity = await _seoRepository.GetByIdAsync(id, false);
            if (entity == null)
            {
                throw new Exception($"SEO entry with ID {id} not found.");
            }

            _seoRepository.Delete(entity);
            await _seoRepository.SaveChangeAsync();

            return _mapper.Map<SeoDataGetDto>(entity);
        }
    }
}
