using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements
{
    public class SliderService : ISliderService
    {
        private readonly ISliderReposiroty _sliderReposiroty;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IMapper _mapper;

        public SliderService(
           ISliderReposiroty sliderRepository,
           ICloudStorageService cloudStorageService,
           IMapper mapper)
        {
            _sliderReposiroty = sliderRepository;
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
        }

        public async Task CreateAsync(SliderCreateDto dto)
        {
            var slider = _mapper.Map<Slider>(dto);

            if (dto.Image == null)
                throw new Exception("Şəkil göndərilməyib");

            slider.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "sliders");

            await _sliderReposiroty.AddAsync(slider);
            await _sliderReposiroty.SaveChangeAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var slider = await _sliderReposiroty.GetByIdAsync(id);
            if (slider == null)
                throw new Exception("Slider tapılmadı");

            await _cloudStorageService.DeleteFileAsync(slider.ImagePath);
            _sliderReposiroty.Delete(slider);
            await _sliderReposiroty.SaveChangeAsync();
        }

        public async Task<ICollection<SliderGetDto>> GetAllAsync()
        {
            var sliders = await _sliderReposiroty.GetAll(false).ToListAsync();
            return _mapper.Map<ICollection<SliderGetDto>>(sliders);
        }

        public async Task<PaginationDto<SliderGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted)
        {
            if (page <= 0 || take <= 0 || order <= 0 || order > 2)
                throw new Exception("Filter parametrləri yanlışdır");

            double count = await _sliderReposiroty.CountAsync(x => !string.IsNullOrEmpty(search) ? x.AltText.ToLower().Contains(search.ToLower()) : true, isDeleted);

            ICollection<Slider> sliders = order switch
            {
                1 => await _sliderReposiroty
                    .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.AltText.ToLower().Contains(search.ToLower()), x => x.CreatedAt, false, isDeleted, (page - 1) * take, take)
                    .ToListAsync(),
                2 => await _sliderReposiroty
                    .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.AltText.ToLower().Contains(search.ToLower()), x => x.CreatedAt, true, isDeleted, (page - 1) * take, take)
                    .ToListAsync(),
                _ => throw new Exception("Sıralama parametri yanlışdır")
            };

            var dtos = _mapper.Map<ICollection<SliderGetDto>>(sliders);

            return new PaginationDto<SliderGetDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<SliderGetDto> GetByIdAsync(string id)
        {
            var slider = await _sliderReposiroty.GetByIdAsync(id);
            if (slider == null)
                throw new Exception("Slider tapılmadı");

            return _mapper.Map<SliderGetDto>(slider);
        }

        public async Task SoftDeleteAsync(string id, bool isDeleted)
        {
            var slider = await _sliderReposiroty.GetByIdAsync(id);
            if (slider == null)
                throw new Exception("Slider tapılmadı");

            _sliderReposiroty.SoftDelete(slider, isDeleted);
            await _sliderReposiroty.SaveChangeAsync();
        }

        public async Task UpdateAsync(string id, SliderUpdateDto dto)
        {
            var slider = await _sliderReposiroty.GetByIdAsync(id);
            if (slider == null)
                throw new Exception("Slider tapılmadı");

            _mapper.Map(dto, slider);

            if (dto.Image != null)
            {
                await _cloudStorageService.DeleteFileAsync(slider.ImagePath);
                slider.ImagePath = await _cloudStorageService.UploadFileAsync(dto.Image, "sliders");
            }

            _sliderReposiroty.Update(slider);
            await _sliderReposiroty.SaveChangeAsync();
        }
    }
}
