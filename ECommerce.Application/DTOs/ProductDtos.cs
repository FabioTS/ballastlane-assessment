namespace ECommerce.Application.DTOs;

public sealed record CreateProductRequest(string Name, string Description, decimal Price, int StockQuantity);
public sealed record UpdateProductRequest(string Name, string Description, decimal Price, int StockQuantity);
public sealed record ProductResponse(Guid Id, string Name, string Description, decimal Price, int StockQuantity);
public sealed record RegisterUserRequest(string Name, string Email, string Password, string Role);
public sealed record LoginRequest(string Email, string Password);
public sealed record AuthResponse(string Token, string Name, string Email, string Role);
public sealed record UpdateStockRequest(int StockQuantity);
