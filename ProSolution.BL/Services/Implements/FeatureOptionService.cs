using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Characteristics;
using ProSolution.BL.DTOs.FeatureOPtions;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using System.Text;

namespace ProSolution.BL.Services.Implements;

public class FeatureOptionService : IFeatureOptionService
{
    private readonly IFeatureOptionRepository _repo;
    private readonly IMapper _mapper;

    public FeatureOptionService(IFeatureOptionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(FeatureOptionCreateDto dto)
    {
        var entity = _mapper.Map<FeatureOption>(dto);
        entity.Slug = dto.Name?.ToLower().Replace(" ", "-") ?? string.Empty;
        await _repo.AddAsync(entity);
        await _repo.SaveChangeAsync();
    }

    // public async Task UpdateAsync(FeatureOptionUpdateDto dto)
    // {
    //     if (string.IsNullOrWhiteSpace(dto.Id))
    //         throw new NegativIdException();
    //
    //     var entity = await _repo.GetByIdAsync(dto.Id);
    //     if (entity == null)
    //         throw new NotFoundException<FeatureOption>();
    //
    //     _mapper.Map(dto, entity);
    //     entity.Slug = dto.Name?.ToLower().Replace(" ", "-") ?? string.Empty;
    //     _repo.Update(entity);
    //     await _repo.SaveChangeAsync();
    // }
    
    public async Task UpdateAsync(string id, FeatureOptionUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name cannot be empty.");

        string[] include = { $"{nameof(FeatureOption.FeatureOptionItems)}" };

        var featureOption = await _repo.GetByIdAsync(id, true, include);
        if (featureOption == null)
            throw new NotFoundException<FeatureOption>();

        int? oldIndex = featureOption.Index;
        int? newIndex = dto.Index;

        if (newIndex.HasValue && newIndex != oldIndex)
        {
            var sibling = await _repo.GetByExpressionAsync(
                x => x.Index == newIndex && x.ParentId == featureOption.ParentId && x.Id != id
            );

            if (sibling != null)
            {
                sibling.Index = oldIndex;
                _repo.Update(sibling);
            }

            featureOption.Index = newIndex;
        }

        _mapper.Map(dto, featureOption);
        featureOption.Slug = await GenerateSlugAsync(featureOption);

        _repo.Update(featureOption);
        await _repo.SaveChangeAsync();
    }
    
    
        private async Task<string> GenerateSlugAsync(FeatureOption featureOption)
        {
            return featureOption.Name?.ToLower().Replace(" ", "-") ?? string.Empty;
        }
    

    public async Task DeleteAsync(string id)
    {
        string[] includes = {
            $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.Children)}",
            $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.ProductFeatures)}.{nameof(ProductFeature.Product)}"
        };

        var entity = await _repo.GetByIdAsync(id, true, includes);
        if (entity == null)
            throw new NotFoundException<FeatureOption>();

        _repo.Delete(entity);
        await _repo.SaveChangeAsync();
    }

    public async Task SoftDeleteAsync(string id, bool isDeleted)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException<FeatureOption>();

        _repo.SoftDelete(entity, isDeleted);
        await _repo.SaveChangeAsync();
    }

    public async Task<ICollection<FeatureOptionGetDto>> GetAll()
    {
        string[] includes = {
    $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.Children)}",
    $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.ProductFeatures)}.{nameof(ProductFeature.Product)}"
};

        ICollection<FeatureOption> list = await _repo
            .GetAll(false, includes)
            .ToListAsync();

        return _mapper.Map<ICollection<FeatureOptionGetDto>>(list);
    }




    public async Task<FeatureOptionGetDto> GetByIdAsync(string id)
    {
        string[] includes = {
    $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.Children)}",
    $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.ProductFeatures)}.{nameof(ProductFeature.Product)}"
};

        var entity = await _repo.GetByIdAsync(id, false, includes);
        if (entity == null)
            throw new NotFoundException<FeatureOption>();

        return _mapper.Map<FeatureOptionGetDto>(entity);
    }

    public async Task<PaginationDto<FeatureOptionGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted)
    {
        if (page <= 0 || take <= 0 || order <= 0 || order > 2)
            throw new Exception("Filter parametrləri yanlışdır");

        string[] includes = {
                $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.Children)}",
                $"{nameof(FeatureOption.FeatureOptionItems)}.{nameof(FeatureOptionItem.ProductFeatures)}.{nameof(ProductFeature.Product)}"};


        double count = await _repo.CountAsync(x =>
                !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true, isDeleted);

        ICollection<FeatureOption> list = order switch
        {
            1 => await _repo
                .GetAllWhereByOrder(x =>
                string.IsNullOrEmpty(search) ? true : (x.Name.ToLower().Contains(search.ToLower()) ||
                (x.FeatureOptionItems != null && x.FeatureOptionItems.Any(t => t.Name != null && t.Name.ToLower().Contains(search.ToLower())))),
                x => x.CreatedAt, false, isDeleted, (page - 1) * take, take, false, includes)
                .ToListAsync(),

            2 => await _repo
                .GetAllWhereByOrder(x =>
                string.IsNullOrEmpty(search) ? true : (x.Name.ToLower().Contains(search.ToLower()) ||
                (x.FeatureOptionItems != null && x.FeatureOptionItems.Any(t => t.Name != null && t.Name.ToLower().Contains(search.ToLower())))),
                x => x.CreatedAt, true, isDeleted, (page - 1) * take, take, false, includes)
                .ToListAsync(),

            _ => throw new Exception("Sıralama parametri yanlışdır")
        };

        if (!string.IsNullOrEmpty(search))
        {
            foreach (var featureOption in list)
            {
                if (featureOption.FeatureOptionItems != null)
                {
                    featureOption.FeatureOptionItems = featureOption.FeatureOptionItems
                        .Where(item => item.Name != null && item.Name.ToLower().Contains(search.ToLower()))
                        .ToList();
                }
            }
        }

        ICollection<FeatureOptionGetDto> dtos = _mapper.Map<ICollection<FeatureOptionGetDto>>(list);

        return new()
        {
            Take = take,
            Search = search,
            Order = order,
            CurrentPage = page,
            TotalPage = Math.Ceiling(count / take),
            Items = dtos
        };
    }

    // ==== Item Methods ====

    public async Task CreateItemAsync(FeatureOptionItemCreateDto dto)
    {
        var item = _mapper.Map<FeatureOptionItem>(dto);
        item.ParentId = dto.ParentId;
        item.FeatureOptionId = dto.FeatureOptionId;

        // Генерация slug
        item.Slug = await GenerateFullSlugAsync(item);


        if (!string.IsNullOrWhiteSpace(dto.ParentId))
        {
            if (!await _repo.CheckUniqueAsync(x => x.Id.Trim().ToLower().Contains(dto.ParentId!.Trim().ToLower())) &&
                string.IsNullOrEmpty(dto.ParentId))
                throw new NotFoundException<FeatureOptionItem>("Parent not found");
        }
        else
        {
            if (!await _repo.CheckUniqueAsync(x => x.Id.Trim().ToLower().Contains(dto.FeatureOptionId!.Trim().ToLower())) &&
                string.IsNullOrEmpty(dto.FeatureOptionId))
                throw new NotFoundException<FeatureOptionItem>("Parent not found");
        }

        await _repo.AddFeatureOptionItemAsync(item);
        await _repo.SaveChangeAsync();
    }
    public async Task UpdateItemAsync(FeatureOptionItemUpdateDto dto)
    {
        var item = await _repo.GetQueryable<FeatureOptionItem>().FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (item == null)
            throw new NotFoundException<FeatureOptionItem>();

        _mapper.Map(dto, item);

        // Обновление slug при изменении имени
        item.Slug = await GenerateFullSlugAsync(item);


        _repo.UpdateFeatureOptionItemAsync(item);
        await _repo.SaveChangeAsync();
    }


    public async Task DeleteItemAsync(string id)
    {
        string[] includes = { $"{nameof(FeatureOptionItem.Children)}" , $"{nameof(FeatureOptionItem.ProductFeatures)}"
        };

        var item = await _repo.GetByIdFeatureOptionItemAsync(id, true, includes);

        if (item == null)
            throw new NotFoundException<FeatureOptionItem>();

        await DeleteRecursiveAsync(item);

        await _repo.SaveChangeAsync();
    }
    private async Task DeleteRecursiveAsync(FeatureOptionItem item)
    {
        // Əvvəlcə bütün alt uşaqları silirik (rekursiv)
        if (item.Children != null && item.Children.Any())
        {
            foreach (var child in item.Children.ToList())
            {
                await DeleteRecursiveAsync(child);
            }
        }

        _repo.DeleteFeatureOptionItemAsync(item); // əsas item silinir
    }
    private async Task<string> GenerateFullSlugAsync(FeatureOptionItem item)
    {
        string parts = "";

        // Добавляем название FeatureOption, если оно есть
        if (!string.IsNullOrEmpty(item.FeatureOptionId))
        {
            var featureOption = await _repo.GetByExpressionAsync(x => x.Id == item.FeatureOptionId);
            parts = featureOption.Slug ?? string.Empty;
        }

        // Поднимаемся по родителям вверх
        var current = item;
        while (!string.IsNullOrEmpty(current.ParentId))
        {
            current = await _repo.GetQueryable<FeatureOptionItem>()
                .FirstOrDefaultAsync(x => x.Id == current.ParentId);

            if (current == null)
                break;

            parts = parts + "/" + current.Slug;
        }

        parts = parts + "/" + Slugify(item.Name);

        // Формируем slug

        return parts;
    }



    private string Slugify(string name)
    {
        return name
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("ə", "e").Replace("ö", "o").Replace("ü", "u").Replace("ş", "s")
            .Replace("ç", "c").Replace("ğ", "g").Replace("ı", "i")
            .Replace("'", "").Replace("\"", "")
            .Replace(".", "").Replace(",", "").Replace("/", "").Replace("\\", "")
            .Replace("?", "").Replace("!", "").Replace("&", "").Replace(":", "").Replace(";", "")
            .Trim('-');
    }


}
