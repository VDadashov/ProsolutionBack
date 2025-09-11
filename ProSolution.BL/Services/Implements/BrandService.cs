using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using System.Reflection.Metadata;

namespace ProSolution.BL.Services.Implements
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, ICloudStorageService cloudStorageService, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
        }

        public async Task CreateAsync(BrandCreateDto dto)
        {
            if (dto.Image == null)
                throw new NegativIdException("Sekil gonderilmeyib");

            var brand = _mapper.Map<Brand>(dto);
            brand.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "brands");
            brand.Slug = _generateSlug(brand.Title);
            await _brandRepository.AddAsync(brand);
            await _brandRepository.SaveChangeAsync();
        }

        public async Task UpdateAsync(string id, BrandUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new NegativIdException("ID bos ola bilmez");

            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException<Brand>();

            _mapper.Map(dto, brand);

            if (dto.Image != null)
            {
                await _cloudStorageService.DeleteFileAsync(brand.ImagePath);
                brand.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "brands");
            }

            if (!dto.Title.Contains("string") && !string.IsNullOrWhiteSpace(dto.Title))
                brand.Slug = _generateSlug(brand.Title);

            _brandRepository.Update(brand);
            await _brandRepository.SaveChangeAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException<Brand>();

            await _cloudStorageService.DeleteFileAsync(brand.ImagePath);
            _brandRepository.Delete(brand);
            await _brandRepository.SaveChangeAsync();
        }

        public async Task SoftDeleteAsync(string id, bool isDelete)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException<Brand>();

            _brandRepository.SoftDelete(brand, isDelete);
            await _brandRepository.SaveChangeAsync();
        }

        public async Task<ICollection<BrandGetDto>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAll(false).Include(b => b.Products).ToListAsync();
            return _mapper.Map<ICollection<BrandGetDto>>(brands);
        }

        public async Task<PaginationDto<BrandGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted)
        {
            if (page <= 0 || take <= 0 || order <= 0 || order > 2)
                throw new NegativIdException("Filter parametrleri yanlisdir");

            double count = await _brandRepository.CountAsync(x => !string.IsNullOrEmpty(search) ? x.Title.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Brand> brands = order switch
            {
                1 => await _brandRepository
                        .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.Title.ToLower().Contains(search.ToLower()), x => x.CreatedAt, false, isDeleted, (page - 1) * take, take)
                        .ToListAsync(),
                2 => await _brandRepository
                        .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.Title.ToLower().Contains(search.ToLower()), x => x.CreatedAt, true, isDeleted, (page - 1) * take, take)
                        .ToListAsync(),
                _ => throw new NegativIdException("Siralama parametri yanlisdir")
            };

            var dtos = _mapper.Map<ICollection<BrandGetDto>>(brands);

            return new PaginationDto<BrandGetDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<BrandGetDto> GetByIdAsync(string id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new NotFoundException<Brand>();

            return _mapper.Map<BrandGetDto>(brand);
        }

        private string _generateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            // Trim və bütün ardıcıl boşluqları tək boşluğa sal
            var normalized = System.Text.RegularExpressions.Regex.Replace(title.Trim().ToLower(), @"\s+", " ");

            // Sonra boşluqları "-" ilə əvəz et
            var slug = normalized.Replace(" ", "-");

            return "brand/" + slug.ToLowerInvariant();
        }
    }
}
