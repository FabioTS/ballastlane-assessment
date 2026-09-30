using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Services;

public sealed class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthenticationService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new DomainException("User name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
        {
            throw new DomainException("A valid email address is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            throw new DomainException("Password must contain at least 6 characters.");
        }

        var normalizedEmail = request.Email.Trim();
        var exists = await _userRepository.ExistsByEmailAsync(normalizedEmail);
        if (exists)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var role = ParseRole(request.Role);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(request.Name, normalizedEmail, passwordHash, role);

        await _userRepository.AddAsync(user);

        return new AuthResponse(
            _tokenService.GenerateToken(user),
            user.Name,
            user.Email,
            user.Role.ToString());
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
        {
            throw new DomainException("A valid email address is required.");
        }

        var user = await _userRepository.GetByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return new AuthResponse(
            _tokenService.GenerateToken(user),
            user.Name,
            user.Email,
            user.Role.ToString());
    }

    private static Role ParseRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return Role.User;
        }

        return roleName.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase)
            ? Role.Admin
            : Role.User;
    }
}
