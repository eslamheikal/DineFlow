using AuthService.Domain.Entities;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data;

public static class PermissionSeeder
{
    public static async Task SeedPermissionsAsync(AuthDbContext context)
    {
        if (!await context.Set<Permission>().AnyAsync())
        {
            var permissions = new List<Permission>
            {
                // User Management
                Permission.Create("users.view", "users", "view", "View users"),
                Permission.Create("users.create", "users", "create", "Create users"),
                Permission.Create("users.update", "users", "update", "Update users"),
                Permission.Create("users.delete", "users", "delete", "Delete users"),
                Permission.Create("users.manage", "users", "manage", "Manage users (full access)"),

                // Role Management
                Permission.Create("roles.view", "roles", "view", "View roles"),
                Permission.Create("roles.create", "roles", "create", "Create roles"),
                Permission.Create("roles.update", "roles", "update", "Update roles"),
                Permission.Create("roles.delete", "roles", "delete", "Delete roles"),
                Permission.Create("roles.manage", "roles", "manage", "Manage roles (full access)"),

                // Restaurant Management
                Permission.Create("restaurants.view", "restaurants", "view", "View restaurants"),
                Permission.Create("restaurants.create", "restaurants", "create", "Create restaurants"),
                Permission.Create("restaurants.update", "restaurants", "update", "Update restaurants"),
                Permission.Create("restaurants.delete", "restaurants", "delete", "Delete restaurants"),
                Permission.Create("restaurants.manage", "restaurants", "manage", "Manage restaurants (full access)"),

                // Menu Management
                Permission.Create("menus.view", "menus", "view", "View menus"),
                Permission.Create("menus.create", "menus", "create", "Create menus"),
                Permission.Create("menus.update", "menus", "update", "Update menus"),
                Permission.Create("menus.delete", "menus", "delete", "Delete menus"),
                Permission.Create("menus.manage", "menus", "manage", "Manage menus (full access)"),

                // Menu Item Management
                Permission.Create("menu-items.view", "menu-items", "view", "View menu items"),
                Permission.Create("menu-items.create", "menu-items", "create", "Create menu items"),
                Permission.Create("menu-items.update", "menu-items", "update", "Update menu items"),
                Permission.Create("menu-items.delete", "menu-items", "delete", "Delete menu items"),
                Permission.Create("menu-items.manage", "menu-items", "manage", "Manage menu items (full access)"),

                // Order Management
                Permission.Create("orders.view", "orders", "view", "View orders"),
                Permission.Create("orders.create", "orders", "create", "Create orders"),
                Permission.Create("orders.update", "orders", "update", "Update orders"),
                Permission.Create("orders.delete", "orders", "delete", "Delete orders"),
                Permission.Create("orders.manage", "orders", "manage", "Manage orders (full access)"),
                Permission.Create("orders.cancel", "orders", "cancel", "Cancel orders"),
                Permission.Create("orders.process", "orders", "process", "Process orders"),

                // Table Management
                Permission.Create("tables.view", "tables", "view", "View tables"),
                Permission.Create("tables.create", "tables", "create", "Create tables"),
                Permission.Create("tables.update", "tables", "update", "Update tables"),
                Permission.Create("tables.delete", "tables", "delete", "Delete tables"),
                Permission.Create("tables.manage", "tables", "manage", "Manage tables (full access)"),

                // Reservation Management
                Permission.Create("reservations.view", "reservations", "view", "View reservations"),
                Permission.Create("reservations.create", "reservations", "create", "Create reservations"),
                Permission.Create("reservations.update", "reservations", "update", "Update reservations"),
                Permission.Create("reservations.delete", "reservations", "delete", "Delete reservations"),
                Permission.Create("reservations.manage", "reservations", "manage", "Manage reservations (full access)"),

                // Staff Management
                Permission.Create("staff.view", "staff", "view", "View staff"),
                Permission.Create("staff.create", "staff", "create", "Create staff"),
                Permission.Create("staff.update", "staff", "update", "Update staff"),
                Permission.Create("staff.delete", "staff", "delete", "Delete staff"),
                Permission.Create("staff.manage", "staff", "manage", "Manage staff (full access)"),

                // Reports
                Permission.Create("reports.view", "reports", "view", "View reports"),
                Permission.Create("reports.generate", "reports", "generate", "Generate reports"),
                Permission.Create("reports.export", "reports", "export", "Export reports"),

                // Settings
                Permission.Create("settings.view", "settings", "view", "View settings"),
                Permission.Create("settings.update", "settings", "update", "Update settings"),
                Permission.Create("settings.manage", "settings", "manage", "Manage settings (full access)"),

                // System
                Permission.Create("system.full-access", "*", "*", "Full system access")
            };

            await context.Set<Permission>().AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }
    }
} 