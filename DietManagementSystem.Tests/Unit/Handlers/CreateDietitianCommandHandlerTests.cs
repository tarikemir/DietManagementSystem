using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Dietitian.CreateDietitian;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Application.Common;
using Moq;
using Xunit;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Application.Features.Auth.RegisterDietitian;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class CreateDietitianCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IRepository<Dietitian>> _dietitianRepoMock = new();

    private CreateDietitianCommandHandler CreateHandler()
    {
        _unitOfWorkMock.Setup(u => u.Dietitians).Returns(_dietitianRepoMock.Object);
        return new CreateDietitianCommandHandler(_unitOfWorkMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenDietitianCreated()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateDietitianCommand
        {
            Email = "dietitian@example.com",
            Password = "password",
            FirstName = "Jane",
            LastName = "Doe"
        };
        var userId = Guid.NewGuid();
        var registerResult = Result<RegisterDietitianCommandResponse>.Success(
            new RegisterDietitianCommandResponse { UserId = userId }
        );
        var dietitian = new Dietitian
        {
            ApplicationUserId = userId,
            FirstName = "Jane",
            LastName = "Doe",
            CreatedAt = DateTime.UtcNow
        };

        _authServiceMock.Setup(s => s.RegisterDietitianAsync(It.IsAny<Application.Features.Auth.RegisterDietitian.RegisterDietitianCommand>()))
            .ReturnsAsync(registerResult);
        _dietitianRepoMock.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(dietitian);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(dietitian.FirstName, result.Value.FirstName);
        Assert.Equal(dietitian.LastName, result.Value.LastName);
        Assert.Equal(dietitian.ApplicationUserId, result.Value.ApplicationUserId);
    }

    [Fact]
    public async Task Handle_Throws_WhenRegisterDietitianFails()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateDietitianCommand
        {
            Email = "dietitian@example.com",
            Password = "password",
            FirstName = "Jane",
            LastName = "Doe"
        };
        var failureResult = Result<RegisterDietitianCommandResponse>.Failure("Registration failed");
        _authServiceMock.Setup(s => s.RegisterDietitianAsync(It.IsAny<Application.Features.Auth.RegisterDietitian.RegisterDietitianCommand>()))
            .ReturnsAsync(failureResult);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));
    }
}
