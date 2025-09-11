using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;

namespace ProSolution.DAL.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DbSet<CategoryProduct> _dbCategoryProduct;
        private readonly DbSet<Category> _dbSet;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _dbCategoryProduct = context.Set<CategoryProduct>();
            _dbSet = context.Set<Category>();

        }

        public async Task<int?> ProductsCountAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            id = id.Trim().ToLower();

            // Получаем все категории
            var allCategories = await _dbSet.ToListAsync();

            // Получаем все дочерние категории рекурсивно
            var categoryIds = GetAllSubCategoryIds(id, allCategories);
            categoryIds.Add(id); // включаем саму категорию

            // Считаем продукты, привязанные ко всем этим категориям
            int productCounts = await _dbCategoryProduct
                .CountAsync(x => categoryIds.Contains(x.CategoryId.ToLower()));

            return productCounts;
        }

        public async Task<int> ChildrensCountRecursiveAsync(string categoryId)
        {
            var allCategories = await _dbSet.ToListAsync();

            var childIds = GetAllSubCategoryIds(categoryId, allCategories);

            return childIds.Count;
        }


        private List<string> GetAllSubCategoryIds(string parentId, List<Category> allCategories)
        {
            var result = new List<string>();
            var children = allCategories
                .Where(c => c.ParentId != null && c.ParentId.ToLower() == parentId)
                .ToList();

            foreach (var child in children)
            {
                result.Add(child.Id.ToLower());
                result.AddRange(GetAllSubCategoryIds(child.Id.ToLower(), allCategories));
            }

            return result;
        }


    }
}
