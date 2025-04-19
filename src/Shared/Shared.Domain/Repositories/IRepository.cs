using Shared.Domain.Common;
using System.Linq.Expressions;

namespace Shared.Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    void Add(T entity);
    void Attach(T entity);
    void Update(T entity);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate = null!);
    Task<List<T>> GetListReadOnlyAsync(Expression<Func<T, bool>> predicate = null!, Expression<Func<T, object>>[]? includes = null);
    public void Delete(int id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task AddRangeAsync(List<T> entities);
}
