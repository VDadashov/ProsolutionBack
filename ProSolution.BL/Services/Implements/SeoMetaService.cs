using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements
{
    public class SeoMetaService : ISeoMetaService
    {
        private readonly ISeoDataMetaRepository _seoMetaRepository;
        private readonly IMapper _mapper;

        public SeoMetaService(ISeoDataMetaRepository seoMetaRepository, IMapper mapper)
        {
            _seoMetaRepository = seoMetaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SEOMetaDTO>> GetAllAsync()
        {
            var entities = await _seoMetaRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SEOMetaDTO>>(entities);
        }

        public async Task<SEOMetaDTO> GetByIdAsync(string id)
        {
            var entity = await _seoMetaRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"SEO Meta entry with ID {id} not found.");
            }

            return _mapper.Map<SEOMetaDTO>(entity);
        }

        public async Task<SEOMetaDTO> AddAsync(CreateSEOMetaDTO dto)
        {
            var allEntities = await _seoMetaRepository.GetAll()
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(dto.MetaTitle) &&
                allEntities.Any(e => e.MetaTitle == dto.MetaTitle))
            {
                throw new InvalidOperationException($"MetaTitle '{dto.MetaTitle}' already exists.");
            }

            if (!string.IsNullOrWhiteSpace(dto.MetaDescription) &&
                allEntities.Any(e => e.MetaDescription == dto.MetaDescription))
            {
                throw new InvalidOperationException($"MetaDescription '{dto.MetaDescription}' already exists.");
            }

            var entity = _mapper.Map<SeoMeta>(dto);
            await _seoMetaRepository.AddAsync(entity);
            await _seoMetaRepository.SaveChangeAsync();

            return _mapper.Map<SEOMetaDTO>(entity);
        }

        public async Task<SEOMetaDTO> UpdateAsync(string id, UpdateSEOMetaDTO dto)
        {
            var entity = await _seoMetaRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"SEO Meta entry with ID {id} not found.");
            }

            _mapper.Map(dto, entity);
            _seoMetaRepository.Update(entity);
            await _seoMetaRepository.SaveChangeAsync();

            return _mapper.Map<SEOMetaDTO>(entity);
        }

        public async Task<SEOMetaDTO> DeleteAsync(string id)
        {
            var entity = await _seoMetaRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"SEO Meta entry with ID {id} not found.");
            }

            _seoMetaRepository.Delete(entity);
            await _seoMetaRepository.SaveChangeAsync();

            return _mapper.Map<SEOMetaDTO>(entity);
        }
    }
}
