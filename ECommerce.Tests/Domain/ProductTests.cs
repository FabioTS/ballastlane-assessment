using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Tests;

public class ProductTests
{
    [Fact]
    public void Product_ShouldRejectZeroPrice()
    {
        var exception = Assert.Throws<DomainException>(() => new Product("Laptop", "Description", 0m, 1));
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void Product_ShouldRejectNegativeStock()
    {
        var exception = Assert.Throws<DomainException>(() => new Product("Laptop", "Description", 1m, -1));
        Assert.Contains("cannot be negative", exception.Message);
    }
}
