using Microsoft.EntityFrameworkCore;
using ProSolution.Core.Entities.Commons;
using ProSolution.Core.Repositories.Common;
using ProSolution.DAL.Contexts;
using System.Linq.Expressions;
namespace ProSolution.DAL.Repositories.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
{
    private readonly DbSet<T> _dbSet;
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
    {
        return _context.Set<TEntity>().AsQueryable();
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
    public async Task<double> CountAsync(Expression<Func<T, bool>>? expression = null, bool IsDeleted = false)
    {
        var query = _dbSet.AsQueryable();
        if (expression != null) query = query.Where(expression);

        if (IsDeleted) query = query.Where(x => x.IsDeleted == true);
        else query = query.Where(x => x.IsDeleted == false);

        return await query.CountAsync();
    }

    public async Task<bool> CheckUniqueAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public IQueryable<T> GetAll(bool IsTracking = true, params string[] includes)
    {
        var query = _dbSet.AsQueryable();

        query = _addIncludes(query, includes);

        return IsTracking ? query : query.AsNoTracking();
    }

    public IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 0,
        bool IsTracking = true, params string[] includes)
    {
        var query = _dbSet.AsQueryable();
        if (expression != null) query = query.Where(expression);

        if (skip != 0) query = query.Skip(skip);

        if (take != 0) query = query.Take(take);

        query = _addIncludes(query, includes);

        return IsTracking ? query : query.AsNoTracking();
    }

    public IQueryable<T> GetAllWhereByOrder(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderException = null,
        bool IsDescending = false, bool IsDeleted = false, int skip = 0, int take = 0, bool IsTracking = true, params string[] includes)
    {
        var query = _dbSet.AsQueryable();
        if (expression != null) query = query.Where(expression);

        if (orderException != null)
        {
            if (IsDescending) query = query.OrderByDescending(orderException);
            else query = query.OrderBy(orderException);
        }
        if (IsDeleted) query = query.Where(x => x.IsDeleted == true);

        else query = query.Where(x => x.IsDeleted == false);

        if (skip != 0) query = query.Skip(skip);

        if (take != 0) query = query.Take(take);

        query = _addIncludes(query, includes);

        return IsTracking ? query : query.AsNoTracking();
    }

    public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool IsTracking = true, params string[] includes)
    {
        var query = _dbSet.Where(expression).AsQueryable();

        query = _addIncludes(query, includes);

        if (!IsTracking) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T> GetByIdAsync(string id, bool IsTracking = true, params string[] includes)
    {
        var query = _dbSet.Where(x => x.Id.Contains(id)).AsQueryable();

        query = _addIncludes(query, includes);

        if (!IsTracking) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void SoftDelete(T entity, bool isDeleted)
    {
        entity.IsDeleted = isDeleted;
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    private IQueryable<T> _addIncludes(IQueryable<T> query, params string[] includes)
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
