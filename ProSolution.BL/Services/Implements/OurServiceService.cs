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
    public class OurServiceService : IOurServiceService
    {
        private readonly IOurServiceRepository _ourServiceRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IMapper _mapper;

        public OurServiceService(IOurServiceRepository ourServiceRepository, ICloudStorageService cloudStorageService, IMapper mapper)
        {
            _ourServiceRepository = ourServiceRepository;
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
        }

        public async Task CreateAsync(OurServiceCreateDto dto)
        {
            if (dto.Image == null || dto.ContentImage == null)
                throw new Exception("Şəkillər göndərilməyib");

            var service = _mapper.Map<OurService>(dto);

            service.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "ourservices");
            service.ContentPath = await _cloudStorageService.UploadFileAsync(dto.ContentImage, "ourservices");

            await _ourServiceRepository.AddAsync(service);
            await _ourServiceRepository.SaveChangeAsync();
        }

        public async Task UpdateAsync(string id, OurServiceUpdateDto dto)
        {
            var service = await _ourServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new NotFoundException<OurService>();

            _mapper.Map(dto, service);

            if (dto.Image != null)
            {
                await _cloudStorageService.DeleteFileAsync(service.ImagePath);
                service.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "ourservices");
            }

            if (dto.ContentImage != null)
            {
                await _cloudStorageService.DeleteFileAsync(service.ContentPath);
                service.ContentPath = await _cloudStorageService.UploadFileAsync(dto.ContentImage, "ourservices");
            }

            _ourServiceRepository.Update(service);
            await _ourServiceRepository.SaveChangeAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var service = await _ourServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new NotFoundException<OurService>();

            await _cloudStorageService.DeleteFileAsync(service.ImagePath);
            await _cloudStorageService.DeleteFileAsync(service.ContentPath);

            _ourServiceRepository.Delete(service);
            await _ourServiceRepository.SaveChangeAsync();
        }

        public async Task SoftDeleteAsync(string id, bool isDelete)
        {
            var service = await _ourServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new NotFoundException<OurService>();

            _ourServiceRepository.SoftDelete(service, isDelete);
            await _ourServiceRepository.SaveChangeAsync();
        }

        public async Task<ICollection<OurServiceGetDto>> GetAllAsync()
        {
            var services = await _ourServiceRepository.GetAll(false).ToListAsync();
            return _mapper.Map<ICollection<OurServiceGetDto>>(services);
        }

        public async Task<OurServiceGetDto> GetByIdAsync(string id)
        {
            var service = await _ourServiceRepository.GetByIdAsync(id);
            if (service == null)
                throw new NotFoundException<OurService>();

            return _mapper.Map<OurServiceGetDto>(service);
        }

        public async Task<PaginationDto<OurServiceGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted)
        {
            if (page <= 0 || take <= 0 || order <= 0 || order > 2)
                throw new Exception("Filter parametrləri yanlışdır");

            double count = await _ourServiceRepository.CountAsync(x =>
                !string.IsNullOrEmpty(search) ? x.Title.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<OurService> services = order switch
            {
                1 => await _ourServiceRepository
                    .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.Title.ToLower().Contains(search.ToLower()), x => x.CreatedAt, false, isDeleted, (page - 1) * take, take)
                    .ToListAsync(),
                2 => await _ourServiceRepository
                    .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.Title.ToLower().Contains(search.ToLower()), x => x.CreatedAt, true, isDeleted, (page - 1) * take, take)
                    .ToListAsync(),
                _ => throw new Exception("Sıralama parametri yanlışdır")
            };

            var dtos = _mapper.Map<ICollection<OurServiceGetDto>>(services);

            return new PaginationDto<OurServiceGetDto>
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
