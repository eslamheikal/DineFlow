using Microsoft.EntityFrameworkCore;
using Shared.Domain.Common;
using Shared.Domain.Repositories;
using System.Linq.Expressions;

namespace AuthService.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _entities;
    public Repository(DbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }
    protected virtual IQueryable<T> Entities { get => _entities; }
    protected IQueryable<E> GetEntiry<E>() where E : class
    {
        return _context.Set<E>();
    }

    protected DbSet<E> GetDbSet<E>() where E : class
    {
        return _context.Set<E>();
    }

    public void Add(T entity)
    {
        _entities.Add(entity);
    }

    public void Attach(T entity)
    {
        _entities.Attach(entity);
    }

    public void Update(T entity)
    {
        _entities.Update(entity);
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        return query.FirstOrDefaultAsync(predicate)!;
    }

    public Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate = null!)
    {
        return predicate != null ? Entities.Where(predicate).ToListAsync() : Entities.ToListAsync();
    }

    public Task<List<T>> GetListReadOnlyAsync(Expression<Func<T, bool>> predicate = null!, Expression<Func<T, object>>[]? includes = null)
    {
        var query = Include(includes).AsNoTrackingWithIdentityResolution();
        return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
    }

    protected IQueryable<T> Include(Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Entities;
        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return query;
    }

    public void Delete(int id)
    {
        var entity = _entities.Find(id);
        if (entity != null)
        {
            _entities.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await Entities.AnyAsync(predicate);
    }

    public async Task AddRangeAsync(List<T> entities)
    {
        await _entities.AddRangeAsync(entities);
    }
}
