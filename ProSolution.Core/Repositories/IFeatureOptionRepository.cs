using ProSolution.Core.Entities;
using ProSolution.Core.Repositories.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProSolution.Core.Repositories
{
    public interface IFeatureOptionRepository : IGenericRepository<FeatureOption>
    {
        Task AddFeatureOptionItemAsync(FeatureOptionItem featureOptionItem);
        void UpdateFeatureOptionItemAsync(FeatureOptionItem featureOptionItem);
        void DeleteFeatureOptionItemAsync(FeatureOptionItem featureOptionItem);
        Task<FeatureOptionItem> GetByIdFeatureOptionItemAsync(string id, bool IsTracking = true, params string[] includes);
        Task<FeatureOptionItem> GetByIdFeatureOptionItemExpressionAsync(Expression<Func<FeatureOptionItem, bool>> expression, bool IsTracking = true, params string[] includes);

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync(IDbContextTransaction transaction);
        Task RollbackAsync(IDbContextTransaction transaction);


    }
}
