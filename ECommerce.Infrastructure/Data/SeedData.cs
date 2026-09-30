using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        var userPasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!");

        var adminUser = User.Create("System Admin", "admin@ecommerce.local", adminPasswordHash, Role.Admin);
        var regularUser = User.Create("Demo User", "user@ecommerce.local", userPasswordHash, Role.User);

        var products = new[]
        {
            new Product("Laptop Pro", "Premium notebook for work and travel.", 1499.99m, 12),
            new Product("Wireless Mouse", "Ergonomic mouse with silent buttons.", 49.99m, 40),
            new Product("Mechanical Keyboard", "Compact keyboard with tactile switches.", 119.99m, 20),
            new Product("4K Monitor", "27-inch display with crisp color accuracy.", 349.99m, 8)
        };

        await context.Users.AddRangeAsync(adminUser, regularUser);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
