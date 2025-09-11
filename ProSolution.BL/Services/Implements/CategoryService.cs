using AutoMapper;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using ProSolution.BL.DTOs;
using ProSolution.BL.DTOs.Categories.CategoryItemDTO;
using ProSolution.BL.Exceptions.Common;
using ProSolution.BL.Services.Interfaces;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using System.Collections;

namespace ProSolution.BL.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CategoryCreateDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.Slug = await _categoryPathSlug(category);

            // Sonuncu index təyin olunur
            var maxIndex = await _categoryRepository
                .GetAll()
                .Where(x => x.ParentId == category.ParentId && !x.IsDeleted)
                .MaxAsync(x => (int?)x.Index) ?? 0;

            category.Index = maxIndex + 1;

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangeAsync();
        }

        public async Task UpdateAsync(string id, CategoryUpdateDto dto)
        {
            string[] include = { $"{nameof(Category.Children)}" };

            var category = await _categoryRepository.GetByIdAsync(id, true, include);
            if (category == null)
                throw new NotFoundException<Category>();

            int? oldIndex = category.Index;
            int? newIndex = dto.Index;

            // Əgər yeni index varsa və fərqlidirsə
            if (newIndex.HasValue && newIndex != oldIndex)
            {
                var sibling = await _categoryRepository.GetByExpressionAsync(
                    x => x.Index == newIndex && x.ParentId == category.ParentId && x.Id != id
                );

                if (sibling != null)
                {
                    // Yerlər dəyişdirilir
                    sibling.Index = oldIndex;
                    _categoryRepository.Update(sibling);
                }

                category.Index = newIndex;
            }

            _mapper.Map(dto, category);
            category.Slug = await _categoryPathSlug(category);

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangeAsync();
        }

        public async Task DeleteAsync(string id)
        {
            string[] include = { $"{nameof(Category.Children)}" };

            var category = await _categoryRepository.GetByIdAsync(id, true, include);
            if (category == null)
                throw new NotFoundException<Category>();

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangeAsync();
        }

        public async Task SoftDeleteAsync(string id, bool isDelete)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException<Category>();

            _categoryRepository.SoftDelete(category, isDelete);
            await _categoryRepository.SaveChangeAsync();
        }

        public async Task<ICollection<CategoryGetDto>> GetAllAsync(string? search, bool isDeleted)
        {
            string[] includes = { $"{nameof(Category.Children)}" };

            var allCategories = await _categoryRepository
                .GetAllWhereByOrder(
                    x => string.IsNullOrEmpty(search) || x.Title.ToLower().Contains(search.ToLower()),
                    x => x.Index ?? int.MaxValue, // null olanlar sona getsin
                    isDeleted: isDeleted,
                    includes: includes
                )
                .ToListAsync();

            var rootCategories = allCategories
                .Where(c => c.ParentId == null)
                .OrderBy(c => c.Index ?? int.MaxValue)
                .ToList();

            var result = rootCategories
                .Select(c => BuildCategoryTreeByIndex(c, allCategories))
                .ToList();

            var dtos = _mapper.Map<ICollection<CategoryGetDto>>(result);

            await SetProductCountsRecursiveAsync(dtos);

            return dtos;
        }


        public async Task<PaginationDto<CategoryGetDto>> GetAllFilteredAsync(string? search, int take, int page, int order, bool isDeleted)
        {
            if (page <= 0 || take <= 0 || order <= 0 || order > 4)
                throw new Exception("Filter parametrləri yanlışdır");

            string[] includes = { $"{nameof(Category.Children)}" };

            double count = await _categoryRepository.CountAsync(x =>
                !string.IsNullOrEmpty(search) ? x.Title.ToLower().Contains(search.ToLower()) : true,
                isDeleted);
            ICollection<Category> allCategories = new List<Category>();
            allCategories = order switch
            {
                1 => await _categoryRepository
                    .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Title.ToLower().Contains(search.ToLower()) : true) &&
                    x.ParentId == null, x => x.Title, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync(),

                2 => await _categoryRepository
                    .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Title.ToLower().Contains(search.ToLower()) : true) &&
                    x.ParentId == null, x => x.Title, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync(),

                3 => await _categoryRepository
                    .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Title.ToLower().Contains(search.ToLower()) : true) &&
                    x.ParentId == null, x => x.CreatedAt, false, isDeleted, (page - 1) * take, take, false, includes).ToListAsync(),

                4 => await _categoryRepository
                    .GetAllWhereByOrder(x => (!string.IsNullOrEmpty(search) ? x.Title.ToLower().Contains(search.ToLower()) : true) &&
                    x.ParentId == null, x => x.CreatedAt, true, isDeleted, (page - 1) * take, take, false, includes).ToListAsync(),

                _ => throw new Exception("Sıralama parametri yanlışdır")
            };

            var rootCategories = allCategories.Where(c => c.ParentId == null).ToList();

            var result = rootCategories.Select(c => BuildCategoryTree(c, allCategories)).ToList();

            var dtos = _mapper.Map<ICollection<CategoryGetDto>>(result);

            foreach (var itemId in dtos)
            {
                itemId.ProductCount =  await _categoryRepository.ProductsCountAsync(itemId.Id!);
            }

            return new PaginationDto<CategoryGetDto>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = dtos
            };
        }

        public async Task<CategoryGetDto> GetByIdAsync(string id)
        {
            string[] includes = {
                    $"{nameof(Category.Children)}",
                    $"{nameof(Category.CategoryProducts)}.{nameof(CategoryProduct.Product)}",
                };

            // Bütün category-ləri yığırıq (tracking off)
            var allCategories = await _categoryRepository
                .GetAll(false, includes)
                .ToListAsync();

            // İstədiyimiz category-ni tapırıq
            var category = allCategories.FirstOrDefault(x => x.Id == id);
            if (category == null)
                throw new NotFoundException<Category>();

            // Rekursiv olaraq children-ləri də daxil edərək sırayla qururuq
            var result = BuildCategoryTreeByIndex(category, allCategories);

            // DTO-ya map olunur
            var dto = _mapper.Map<CategoryGetDto>(result);
            dto.ProductCount = await _categoryRepository.ProductsCountAsync(category.Id!);
            dto.ChildCount = await _categoryRepository.ChildrensCountRecursiveAsync(category.Id!);

            return dto;
        }

        // ====================
        // CategoryItem methods
        // ====================

        public async Task CreateItemAsync(CategoryItemCreateDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            entity.Slug = await _categoryPathSlug(entity);
            await _categoryRepository.AddAsync(entity);
            await _categoryRepository.SaveChangeAsync();
        }


        public async Task UpdateItemAsync(string id, CategoryItemUpdateDto dto)
        {
            var item = await _categoryRepository.GetQueryable<Category>().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                throw new NotFoundException<Category>();

            _mapper.Map(dto, item);
            item.Slug = await _categoryPathSlug(item);

            _categoryRepository.Update(item);
            await _categoryRepository.SaveChangeAsync();
        }

        public async Task DeleteItemAsync(string id)
        {
            var item = await _categoryRepository.GetQueryable<Category>().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                throw new NotFoundException<Category>();

            _categoryRepository.Delete(item);
            await _categoryRepository.SaveChangeAsync();
        }

        private Category BuildCategoryTree(Category parent, ICollection<Category> all)
        {
            parent.Children = all
                .Where(c => c.ParentId == parent.Id)
                .OrderBy(c => c.Title).ToList();
            foreach (var child in parent.Children)
            {
                BuildCategoryTree(child, all);
            }

            return parent;
        }

        private Category BuildCategoryTreeByIndex(Category parent, ICollection<Category> all)
        {
            parent.Children = all
                .Where(c => c.ParentId == parent.Id)
                .OrderBy(c => c.Index ?? int.MaxValue) // null olanlar sona
                .ToList();

            foreach (var child in parent.Children)
            {
                BuildCategoryTreeByIndex(child, all);
            }

            return parent;
        }


        private async Task<string> _categoryPathSlug(Category item)
        {
            var parts = new List<string>();

            while (item != null)
            {
                parts.Add(item.Title.ToLower()); // burada heç bir dəyişiklik (slugify) yoxdur
                if (string.IsNullOrWhiteSpace(item.ParentId)) break;

                item = await _categoryRepository.GetByExpressionAsync(
                    x => x.Id == item.ParentId,
                    includes: $"{nameof(Category.Parent)}"
                );
            }

            parts.Reverse();
            return string.Join("/", parts); // məsələn: Elektronika/Telefonlar/Aksesuarlar
        }


        private async Task SetProductCountsRecursiveAsync(ICollection<CategoryGetDto> categories)
        {
            foreach (var category in categories)
            {
                category.ProductCount = await _categoryRepository.ProductsCountAsync(category.Id!);

                if (category.CategoryItems != null && category.CategoryItems.Any())
                {
                    await SetProductCountsRecursiveAsync(category.CategoryItems);
                }
            }
        }
    }
}
