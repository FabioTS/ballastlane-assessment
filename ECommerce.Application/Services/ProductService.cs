using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Services;

public sealed class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateAsync(CreateProductRequest request)
    {
        ValidateProductRequest(request.Name, request.Description, request.Price, request.StockQuantity);

        var product = new Product(request.Name, request.Description, request.Price, request.StockQuantity);
        await _productRepository.AddAsync(product);
        return product;
    }

    public async Task<Product> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var current = await _productRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Product '{id}' was not found.");

        ValidateProductRequest(request.Name, request.Description, request.Price, request.StockQuantity);

        current.Update(request.Name, request.Description, request.Price, request.StockQuantity);
        await _productRepository.UpdateAsync(current);
        return current;
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Product '{id}' was not found.");

        await _productRepository.DeleteAsync(product.Id);
    }

    public async Task<Product> UpdateStockAsync(Guid id, int stockQuantity)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Product '{id}' was not found.");

        if (stockQuantity < 0)
        {
            throw new DomainException("Stock quantity cannot be negative.");
        }

        product.UpdateStock(stockQuantity);
        await _productRepository.UpdateAsync(product);
        return product;
    }

    private static void ValidateProductRequest(string name, string description, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name is required.");
        }

        if (description is null)
        {
            throw new DomainException("Product description is required.");
        }

        if (price <= 0)
        {
            throw new DomainException("Price must be greater than zero.");
        }

        if (stockQuantity < 0)
        {
            throw new DomainException("Stock quantity cannot be negative.");
        }
    }
}
