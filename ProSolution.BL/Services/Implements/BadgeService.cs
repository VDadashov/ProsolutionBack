using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;

namespace ProSolution.BL.Services.Implements;

public class BadgeService : IBadgeService
{
    private readonly IBadgeRepository _badgeRepository;
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IMapper _mapper;

    public BadgeService(
        IBadgeRepository badgeRepository,
        ICloudStorageService cloudStorageService,
        IMapper mapper)
    {
        _badgeRepository = badgeRepository;
        _cloudStorageService = cloudStorageService;
        _mapper = mapper;
    }

    public async Task CreateAsync(BadgeCreateDto dto)
    {
        var badge = _mapper.Map<Badge>(dto);

        if (dto.Image == null)
            throw new Exception("Şəkil göndərilməyib");

        badge.ImageUrl = await _cloudStorageService.UploadFileAsync(dto.Image, "badges");

        await _badgeRepository.AddAsync(badge);
        await _badgeRepository.SaveChangeAsync();
    }

    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new NegativIdException();

        var badge = await _badgeRepository.GetByIdAsync(id);
        if (badge == null)
            throw new NotFoundException<Badge>();

        await _cloudStorageService.DeleteFileAsync(badge.ImageUrl);
        _badgeRepository.Delete(badge);
        await _badgeRepository.SaveChangeAsync();
    }

    public async Task<ICollection<BadgeGetDto>> GetAllAsync()
    {
        var badges = await _badgeRepository.GetAll(false).ToListAsync();
        return _mapper.Map<ICollection<BadgeGetDto>>(badges);
    }

    public async Task<PaginationDto<BadgeGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted)
    {
        if (page <= 0 || take <= 0 || order <= 0 || order > 2)
            throw new Exception("Filter parametrləri yanlışdır");

        double count = await _badgeRepository.CountAsync(
            x => !string.IsNullOrEmpty(search) ? x.Description.ToLower().Contains(search.ToLower()) : true,
            isDeleted);

        ICollection<Badge> badges = order switch
        {
            1 => await _badgeRepository
                .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.Description.ToLower().Contains(search.ToLower()),
                                    x => x.CreatedAt, false, isDeleted, (page - 1) * take, take)
                .ToListAsync(),
            2 => await _badgeRepository
                .GetAllWhereByOrder(x => string.IsNullOrEmpty(search) || x.Description.ToLower().Contains(search.ToLower()),
                                    x => x.CreatedAt, true, isDeleted, (page - 1) * take, take)
                .ToListAsync(),
            _ => throw new Exception("Sıralama parametri yanlışdır")
        };

        var dtos = _mapper.Map<ICollection<BadgeGetDto>>(badges);

        return new PaginationDto<BadgeGetDto>
        {
            Take = take,
            Search = search,
            Order = order,
            CurrentPage = page,
            TotalPage = Math.Ceiling(count / take),
            Items = dtos
        };
    }

    public async Task<BadgeGetDto> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new NegativIdException();

        var badge = await _badgeRepository.GetByIdAsync(id);
        if (badge == null)
            throw new NotFoundException<Badge>();

        return _mapper.Map<BadgeGetDto>(badge);
    }

    public async Task SoftDeleteAsync(string id, bool isDeleted)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new NegativIdException();

        var badge = await _badgeRepository.GetByIdAsync(id);
        if (badge == null)
            throw new NotFoundException<Badge>();

        _badgeRepository.SoftDelete(badge, isDeleted);
        await _badgeRepository.SaveChangeAsync();
    }

    public async Task UpdateAsync(string id, BadgeUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new NegativIdException();

        var badge = await _badgeRepository.GetByIdAsync(id);
        if (badge == null)
            throw new NotFoundException<Badge>();

        _mapper.Map(dto, badge);

        if (dto.Image != null)
        {
            await _cloudStorageService.DeleteFileAsync(badge.ImageUrl);
            badge.ImageUrl = await _cloudStorageService.UploadFileAsync(dto.Image, "badges");
        }

        _badgeRepository.Update(badge);
        await _badgeRepository.SaveChangeAsync();
    }
}
