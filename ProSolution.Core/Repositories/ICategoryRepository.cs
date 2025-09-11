using ProSolution.Core.Entities;
using ProSolution.Core.Repositories.Common;

namespace ProSolution.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<int?> ProductsCountAsync(string id);
        Task<int> ChildrensCountRecursiveAsync(string categoryId);
    }
}
