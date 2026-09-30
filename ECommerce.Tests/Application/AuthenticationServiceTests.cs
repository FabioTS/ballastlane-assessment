using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Moq;

namespace ECommerce.Application.Tests;

public class AuthenticationServiceTests
{
    [Fact]
    public async Task RegisterAsync_ShouldCreateUserAndToken()
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.ExistsByEmailAsync("user@example.com")).ReturnsAsync(false);

        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("demo-token");

        var service = new AuthenticationService(userRepository.Object, tokenService.Object);

        var result = await service.RegisterAsync(new RegisterUserRequest("Jane", "user@example.com", "Password123!", "User"));

        Assert.Equal("Jane", result.Name);
        Assert.Equal("demo-token", result.Token);
        userRepository.Verify(x => x.AddAsync(It.Is<User>(u => u.Email == "user@example.com")), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldRejectInvalidPassword()
    {
        var user = User.Create("Jane", "user@example.com", BCrypt.Net.BCrypt.HashPassword("Password123!"), Role.User);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.GetByEmailAsync("user@example.com")).ReturnsAsync(user);
        var tokenService = new Mock<ITokenService>();
        var service = new AuthenticationService(userRepository.Object, tokenService.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(new LoginRequest("user@example.com", "WrongPassword")));
    }
}
