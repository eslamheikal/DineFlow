using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AuthDbContext context) : base(context) { }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await Entities
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<IEnumerable<Role>> GetByNamesAsync(IEnumerable<string> names)
    {
        return await Entities
            .Include(r => r.Permissions)
            .Where(r => names.Contains(r.Name))
            .ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetByPermissionAsync(string resource, string action)
    {
        return await Entities
            .Include(r => r.Permissions)
            .Where(r => r.Permissions.Any(p => p.Resource == resource && p.Action == action))
            .ToListAsync();
    }
}