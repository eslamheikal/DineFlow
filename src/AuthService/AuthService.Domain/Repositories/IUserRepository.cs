using AuthService.Domain.Entities;
using Shared.Domain.Repositories;

namespace AuthService.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<IEnumerable<User>> GetByRoleNameAsync(string roleName);
} 