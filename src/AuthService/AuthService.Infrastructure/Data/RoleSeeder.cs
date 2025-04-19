using AuthService.Domain.Entities;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(AuthDbContext context)
    {
        if (!await context.Set<Role>().AnyAsync())
        {
            var permissions = await context.Set<Permission>().ToListAsync();
            
            var roles = new List<Role>
            {
                Role.Create(
                    name: "SuperAdmin",
                    description: "Super Administrator with full system access",
                    permissions: permissions.Where(p => p.Resource == "*" && p.Action == "*").ToList()
                ),
                Role.Create(
                    name: "Admin",
                    description: "System Administrator",
                    permissions: permissions.Where(p => 
                        (p.Resource == "users" && p.Action == "manage") ||
                        (p.Resource == "roles" && p.Action == "manage") ||
                        (p.Resource == "restaurants" && p.Action == "manage") ||
                        (p.Resource == "menus" && p.Action == "manage") ||
                        (p.Resource == "orders" && p.Action == "manage") ||
                        (p.Resource == "staff" && p.Action == "manage") ||
                        (p.Resource == "reports" && p.Action == "view") ||
                        (p.Resource == "settings" && p.Action == "manage")
                    ).ToList()
                ),
                Role.Create(
                    name: "RestaurantManager",
                    description: "Restaurant Manager",
                    permissions: permissions.Where(p => 
                        (p.Resource == "restaurants" && p.Action == "update") ||
                        (p.Resource == "menus" && p.Action == "manage") ||
                        (p.Resource == "menu-items" && p.Action == "manage") ||
                        (p.Resource == "orders" && p.Action == "manage") ||
                        (p.Resource == "tables" && p.Action == "manage") ||
                        (p.Resource == "reservations" && p.Action == "manage") ||
                        (p.Resource == "staff" && p.Action == "manage") ||
                        (p.Resource == "reports" && p.Action == "view")
                    ).ToList()
                ),
                Role.Create(
                    name: "Staff",
                    description: "Restaurant Staff",
                    permissions: permissions.Where(p => 
                        (p.Resource == "orders" && (p.Action == "view" || p.Action == "update" || p.Action == "process")) ||
                        (p.Resource == "menu-items" && p.Action == "view") ||
                        (p.Resource == "tables" && p.Action == "view") ||
                        (p.Resource == "reservations" && p.Action == "view")
                    ).ToList()
                ),
                Role.Create(
                    name: "Customer",
                    description: "Regular Customer",
                    permissions: permissions.Where(p => 
                        (p.Resource == "restaurants" && p.Action == "view") ||
                        (p.Resource == "menus" && p.Action == "view") ||
                        (p.Resource == "menu-items" && p.Action == "view") ||
                        (p.Resource == "orders" && (p.Action == "create" || p.Action == "view")) ||
                        (p.Resource == "reservations" && (p.Action == "create" || p.Action == "view"))
                    ).ToList()
                )
            };

            await context.Set<Role>().AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}