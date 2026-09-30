using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    private Product()
    {
    }

    public Product(string name, string description, decimal price, int stockQuantity)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateStock(stockQuantity);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void Update(string name, string description, decimal price, int stockQuantity)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidatePrice(price);
        ValidateStock(stockQuantity);

        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void UpdateStock(int stockQuantity)
    {
        ValidateStock(stockQuantity);
        StockQuantity = stockQuantity;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name is required.");
        }
    }

    private static void ValidateDescription(string description)
    {
        if (description is null)
        {
            throw new DomainException("Product description is required.");
        }
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
        {
            throw new DomainException("Product price must be greater than zero.");
        }
    }

    private static void ValidateStock(int stockQuantity)
    {
        if (stockQuantity < 0)
        {
            throw new DomainException("Stock quantity cannot be negative.");
        }
    }
}
