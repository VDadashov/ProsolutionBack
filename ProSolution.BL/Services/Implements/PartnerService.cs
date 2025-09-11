using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IMapper _mapper;

        public PartnerService(IPartnerRepository partnerRepository, ICloudStorageService cloudStorageService, IMapper mapper)
        {
            _partnerRepository = partnerRepository;
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
        }

        public async Task CreateAsync(PartnerCreateDto dto)
        {
            if (dto.Image == null)
                throw new Exception("Şəkil göndərilməyib");

            var partner = _mapper.Map<Partner>(dto);
            partner.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "partners");

            await _partnerRepository.AddAsync(partner);
            await _partnerRepository.SaveChangeAsync();
        }

        public async Task UpdateAsync(string id, PartnerUpdateDto dto)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
                throw new NotFoundException<Partner>();

            _mapper.Map(dto, partner);

            if (dto.Image != null)
            {
                await _cloudStorageService.DeleteFileAsync(partner.ImagePath);
                partner.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "partners");
            }

            _partnerRepository.Update(partner);
            await _partnerRepository.SaveChangeAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
                throw new NotFoundException<Partner>();

            await _cloudStorageService.DeleteFileAsync(partner.ImagePath);
            _partnerRepository.Delete(partner);
            await _partnerRepository.SaveChangeAsync();
        }

        public async Task SoftDeleteAsync(string id, bool isDeleted)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
                throw new NotFoundException<Partner>();

            _partnerRepository.SoftDelete(partner, isDeleted);
            await _partnerRepository.SaveChangeAsync();
        }

        public async Task<ICollection<PartnerGetDto>> GetAllAsync()
        {
            var partners = await _partnerRepository.GetAll(false).ToListAsync();
            return _mapper.Map<ICollection<PartnerGetDto>>(partners);
        }

        public async Task<PartnerGetDto> GetByIdAsync(string id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
                throw new NotFoundException<Partner>();

            return _mapper.Map<PartnerGetDto>(partner);
        }

        public async Task<PaginationDto<PartnerGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted)
        {
            if (page <= 0 || take <= 0 || order <= 0 || order > 2)
                throw new Exception("Filter parametrləri yanlışdır");

            double count = await _partnerRepository.CountAsync(x =>
                !string.IsNullOrEmpty(search) ? x.AltText.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Partner> partners = order switch
            {
                1 => await _partnerRepository
                    .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.AltText.ToLower().Contains(search.ToLower()), x => x.CreatedAt, false, isDeleted, (page - 1) * take, take)
                    .ToListAsync(),
                2 => await _partnerRepository
                    .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.AltText.ToLower().Contains(search.ToLower()), x => x.CreatedAt, true, isDeleted, (page - 1) * take, take)
                    .ToListAsync(),
                _ => throw new Exception("Sıralama parametri yanlışdır")
            };

            var dtos = _mapper.Map<ICollection<PartnerGetDto>>(partners);

            return new PaginationDto<PartnerGetDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }
    }
}
