using AuthService.Domain.Entities;
using Shared.Domain.Repositories;

namespace AuthService.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role> GetByNameAsync(string name);
    Task<IEnumerable<Role>> GetByNamesAsync(IEnumerable<string> names);
    Task<IEnumerable<Role>> GetByIdsAsync(IEnumerable<int> ids);
    Task<IEnumerable<Role>> GetByPermissionAsync(string resource, string action);
} 