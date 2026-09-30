using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Tests;

public class SeedDataTests
{
    [Fact]
    public async Task SeedAsync_ShouldCreateAdminAndDemoUser()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ApplicationDbContext(options);
        await SeedData.SeedAsync(context);

        Assert.Equal(2, await context.Users.CountAsync());
        Assert.Equal(4, await context.Products.CountAsync());
    }
}
