using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements
{
    public class SeoUrlService : ISeoUrlService
    {
        private readonly ISeoDataUrlRepository _seoUrlRepository;
        private readonly IMapper _mapper;

        public SeoUrlService(ISeoDataUrlRepository seoUrlRepository, IMapper mapper)
        {
            _seoUrlRepository = seoUrlRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SEOUrlDTO>> GetAllAsync()
        {
            var entities = await _seoUrlRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<SEOUrlDTO>>(entities);
        }

        public async Task<SEOUrlDTO> GetByIdAsync(string id)
        {
            var entity = await _seoUrlRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"SEO URL entry with ID {id} not found.");
            }

            return _mapper.Map<SEOUrlDTO>(entity);
        }

        public async Task<SEOUrlDTO> AddAsync(CreateSeoUrlDTO dto)
        {
            var entity = _mapper.Map<SeoUrl>(dto);
            await _seoUrlRepository.AddAsync(entity);
            await _seoUrlRepository.SaveChangeAsync();
            return _mapper.Map<SEOUrlDTO>(entity);
           
        }

        public async Task<SEOUrlDTO> UpdateAsync(string id, UpdateSEOUrlDTO dto)
        {
            var entity = await _seoUrlRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"SEO URL entry with ID {id} not found.");
            }

            _mapper.Map(dto, entity);
            _seoUrlRepository.Update(entity);
            await _seoUrlRepository.SaveChangeAsync();
            return _mapper.Map<SEOUrlDTO>(entity);
        }
    
        
        public async Task<SEOUrlDTO> DeleteAsync(string id)
        {
            var entity = await _seoUrlRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"SEO URL entry with ID {id} not found.");
            }
                  _seoUrlRepository.Delete(entity);
            await _seoUrlRepository.SaveChangeAsync();

            return _mapper.Map<SEOUrlDTO>(entity);
        }
    }
}

