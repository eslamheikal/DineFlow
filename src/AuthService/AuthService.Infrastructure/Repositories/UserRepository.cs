using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AuthDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Entities
            .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email.Value == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await Entities.AnyAsync(u => u.Email.Value == email);
    }

    public async Task<IEnumerable<User>> GetByRoleNameAsync(string roleName)
    {
        return await Entities
            .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
            .Where(u => u.Roles.Any(r => r.Name == roleName))
            .ToListAsync();
    }

} 