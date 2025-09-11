using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities;
using ProSolution.Core.Repositories;
using ProSolution.Core.Repositories.Common;
using ProSolution.DAL.Contexts;
using ProSolution.DAL.Repositories.Common;
using System.Linq.Expressions;

namespace ProSolution.DAL.Repositories
{
    public class FeatureOptionRepository : GenericRepository<FeatureOption>, IFeatureOptionRepository
    {
        private readonly DbSet<FeatureOptionItem> _featureOptionItems;

        public FeatureOptionRepository(AppDbContext context) : base(context)
        {
            _featureOptionItems = context.Set<FeatureOptionItem>();
        }

        public async Task AddFeatureOptionItemAsync(FeatureOptionItem featureOptionItem)
        {
            await _featureOptionItems.AddAsync(featureOptionItem);
        }

        public void UpdateFeatureOptionItemAsync(FeatureOptionItem featureOptionItem)
        {
            _featureOptionItems.Update(featureOptionItem);
        }

        public void DeleteFeatureOptionItemAsync(FeatureOptionItem featureOptionItem)
        {
            _featureOptionItems.Remove(featureOptionItem);
        }
        public async Task<FeatureOptionItem> GetByIdFeatureOptionItemAsync(string id, bool IsTracking = true, params string[] includes)
        {
            var query = _featureOptionItems.Where(x => x.Id.Contains(id)).AsQueryable();

            query = _addIncludes(query, includes);

            if (!IsTracking) query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<FeatureOptionItem> GetByIdFeatureOptionItemExpressionAsync(Expression<Func<FeatureOptionItem, bool>> expression, bool IsTracking = true, params string[] includes)
        {
            var query = _featureOptionItems.Where(expression).AsQueryable();

            query = _addIncludes(query, includes);

            if (!IsTracking) query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        private IQueryable<FeatureOptionItem> _addIncludes(IQueryable<FeatureOptionItem> query, params string[] includes)
        {
            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
            }
            return query;
        }
    
    }
}
