using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Auth.RegisterClient;
using DietManagementSystem.Application.Features.Client.CreateClient;
using DietManagementSystem.Application.Features.Client.GetClient;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class CreateClientCommandHandlerTests
{
    private readonly Mock<IClientService> _clientServiceMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();

    private CreateClientCommandHandler CreateHandler() =>
        new(_clientServiceMock.Object, _authServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenRegisterClientFails()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateClientCommand();
        var failureResult = Result<RegisterClientCommandResponse>.Failure("Already exists");
        _authServiceMock.Setup(s => s.RegisterClientAsync(It.IsAny<Application.Features.Auth.RegisterClient.RegisterClientCommand>()))
            .ReturnsAsync(failureResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Client is already created.", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenGetClientFails()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateClientCommand();
        var registerResult = Result<RegisterClientCommandResponse>.Success(new RegisterClientCommandResponse { UserId = Guid.NewGuid() });
        var getClientResult = Result<GetClientByIdQueryResponse>.Failure("Not found");
        _authServiceMock.Setup(s => s.RegisterClientAsync(It.IsAny<Application.Features.Auth.RegisterClient.RegisterClientCommand>()))
            .ReturnsAsync(registerResult);
        _clientServiceMock.Setup(s => s.GetClientAsync(registerResult.Value.UserId))
            .ReturnsAsync(getClientResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Not found", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenAllStepsSucceed()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateClientCommand();
        var userId = Guid.NewGuid();
        var registerResult = Result<RegisterClientCommandResponse>.Success(new RegisterClientCommandResponse { UserId = userId });
        var clientResponse = new GetClientByIdQueryResponse
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            InitialWeight = 70,
            DietitianId = Guid.NewGuid()
        };
        var getClientResult = Result<GetClientByIdQueryResponse>.Success(clientResponse);
        _authServiceMock.Setup(s => s.RegisterClientAsync(It.IsAny<Application.Features.Auth.RegisterClient.RegisterClientCommand>()))
            .ReturnsAsync(registerResult);
        _clientServiceMock.Setup(s => s.GetClientAsync(userId))
            .ReturnsAsync(getClientResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(clientResponse.Id, result.Value.Id);
        Assert.Equal(clientResponse.FirstName, result.Value.FirstName);
        Assert.Equal(clientResponse.LastName, result.Value.LastName);
        Assert.Equal(clientResponse.InitialWeight, result.Value.InitialWeight);
        Assert.Equal(clientResponse.DietitianId, result.Value.DietitianId);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateClientCommand();
        _authServiceMock.Setup(s => s.RegisterClientAsync(It.IsAny<Application.Features.Auth.RegisterClient.RegisterClientCommand>()))
            .ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("An error occurred while processing the request: Unexpected", result.Error);
    }
}
