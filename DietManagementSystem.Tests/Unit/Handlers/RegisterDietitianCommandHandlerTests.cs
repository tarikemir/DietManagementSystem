using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Auth.RegisterDietitian;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class RegisterDietitianCommandHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<ILoggingService> _loggingServiceMock = new();

    private RegisterDietitianCommandHandler CreateHandler() =>
        new(_authServiceMock.Object, _loggingServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenResultIsNull()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new RegisterDietitianCommand();
        _authServiceMock.Setup(s => s.RegisterDietitianAsync(command)).ReturnsAsync((Result<RegisterDietitianCommandResponse>)null!);

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
        var command = new RegisterDietitianCommand();
        var failureResult = Result<RegisterDietitianCommandResponse>.Failure("Registration error");
        _authServiceMock.Setup(s => s.RegisterDietitianAsync(command)).ReturnsAsync(failureResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Registration error", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new RegisterDietitianCommand();
        var response = new RegisterDietitianCommandResponse
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = Guid.NewGuid(),
            Email = "dietitian@example.com",
            UserType = "Dietitian"
        };
        var successResult = Result<RegisterDietitianCommandResponse>.Success(response);
        _authServiceMock.Setup(s => s.RegisterDietitianAsync(command)).ReturnsAsync(successResult);

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
        var command = new RegisterDietitianCommand();
        _authServiceMock.Setup(s => s.RegisterDietitianAsync(command)).ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("An error occurred while processing the request: Unexpected", result.Error);
    }
}
