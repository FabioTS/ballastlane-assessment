using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using Moq;

namespace ECommerce.Application.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldAddProduct_WhenRequestIsValid()
    {
        var repository = new Mock<IProductRepository>();
        var service = new ProductService(repository.Object);

        var result = await service.CreateAsync(new CreateProductRequest("Phone", "New phone", 699m, 10));

        Assert.Equal("Phone", result.Name);
        Assert.Equal(699m, result.Price);
        repository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenProductDoesNotExist()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);
        var service = new ProductService(repository.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync(Guid.NewGuid(), new UpdateProductRequest("Phone", "Updated", 700m, 8)));
    }
}
