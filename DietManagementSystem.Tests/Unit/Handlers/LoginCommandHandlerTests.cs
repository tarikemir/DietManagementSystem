using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DietManagementSystem.Application.Features.Auth.Login;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class LoginCommandHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILoggingService> _loggingServiceMock = new();

    private LoginCommandHandler CreateHandler() =>
        new(_authServiceMock.Object, _mapperMock.Object, _loggingServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenResultIsNull()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new LoginCommand { Email = "test@example.com", Password = "pass" };
        _authServiceMock.Setup(s => s.LoginAsync(command)).ReturnsAsync((Result<LoginCommandResponse>)null!);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Login failed. Please check your credentials and try again.", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenResultIsFailure()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new LoginCommand { Email = "test@example.com", Password = "pass" };
        var failureResult = Result<LoginCommandResponse>.Failure("Invalid credentials");
        _authServiceMock.Setup(s => s.LoginAsync(command)).ReturnsAsync(failureResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid credentials", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new LoginCommand { Email = "test@example.com", Password = "pass" };
        var response = new LoginCommandResponse
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = Guid.NewGuid(),
            Email = "test@example.com",
            UserType = "Dietitian"
        };
        var successResult = Result<LoginCommandResponse>.Success(response);
        _authServiceMock.Setup(s => s.LoginAsync(command)).ReturnsAsync(successResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(response, result.Value);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new LoginCommand { Email = "test@example.com", Password = "pass" };
        _authServiceMock.Setup(s => s.LoginAsync(command)).ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("An error occurred while processing the request: Unexpected", result.Error);
    }
}
