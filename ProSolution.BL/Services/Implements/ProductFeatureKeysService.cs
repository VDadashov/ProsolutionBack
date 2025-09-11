using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs.ProductFeatureKeys;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;

public class ProductFeatureKeysService : IProductFeatureKeysService
{
    private readonly IProductFeatureKeysRepository _repository;
    private readonly IFeatureOptionRepository _featureOptionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductFeatureKeysService(IProductFeatureKeysRepository repository,ICategoryRepository categoryRepository, IMapper mapper,IFeatureOptionRepository featureOptionRepository)
    {
        _repository = repository;
       _categoryRepository = categoryRepository;
        _featureOptionRepository = featureOptionRepository;
        _mapper = mapper;
    }

    public async Task<ProductFeatureKeysGetDto> CreateAsync(ProductFeatureKeysCreateDto dto)
    {
        var featureOptions = await _featureOptionRepository
            .GetAllWhere(x => dto.FeatureOptionIds.Contains(x.Id))
            .ToListAsync();

        var entity = _mapper.Map<ProductFeatureKeys>(dto);
        entity.FeatureOptions = featureOptions;

        await _repository.AddAsync(entity);
        await _repository.SaveChangeAsync();

        return await GetByIdAsync(entity.Id);
    }


    public async Task<ProductFeatureKeysGetDto> UpdateAsync(ProductFeatureKeysUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id, true, nameof(ProductFeatureKeys.FeatureOptions), nameof(ProductFeatureKeys.Category));

        if (entity == null) throw new Exception("Не найдено.");

        entity.CategoryId = dto.CategoryId;
        entity.FeatureOptions = await _featureOptionRepository
            .GetAllWhere(x => dto.FeatureOptionIds.Contains(x.Id))
            .ToListAsync();

        _repository.Update(entity);
        await _repository.SaveChangeAsync();

        return await GetByIdAsync(entity.Id);
    }

    public async Task SoftDeleteAsync(string id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) throw new Exception("Не найдено.");

        entity.IsDeleted = !entity.IsDeleted;

        _repository.Update(entity);
        await _repository.SaveChangeAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) throw new Exception("Не найдено.");

        _repository.Delete(entity);
        await _repository.SaveChangeAsync();
    }

    public async Task<List<ProductFeatureKeysGetDto>> GetAllAsync()
    {
        var items = await _repository.GetAll(true, nameof(ProductFeatureKeys.Category), nameof(ProductFeatureKeys.FeatureOptions)).ToListAsync();

        return _mapper.Map<List<ProductFeatureKeysGetDto>>(items);
    }

    public async Task<ProductFeatureKeysGetDto> GetByIdAsync(string id)
    {
        var entity = await _repository.GetByIdAsync(id, true, nameof(ProductFeatureKeys.Category), nameof(ProductFeatureKeys.FeatureOptions));

        if (entity == null) throw new Exception("Не найдено.");

        return _mapper.Map<ProductFeatureKeysGetDto>(entity);
    }
}
