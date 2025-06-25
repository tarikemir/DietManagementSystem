using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Client.UpdateClient;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class UpdateClientCommandHandlerTests
{
    private readonly Mock<IClientService> _clientServiceMock = new();

    private UpdateClientCommandHandler CreateHandler() =>
        new(_clientServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenUpdateFails()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new UpdateClientCommand();
        var failureResult = Result<UpdateClientCommandResponse>.Failure("Update failed");
        _clientServiceMock.Setup(s => s.UpdateClientAsync(command))
            .ReturnsAsync(failureResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Update failed", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenUpdateSucceeds()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new UpdateClientCommand();
        var response = new UpdateClientCommandResponse
        {
            Id = Guid.NewGuid(),
            FirstName = "Updated",
            LastName = "Client",
            InitialWeight = 75,
            DietitianId = Guid.NewGuid()
        };
        var successResult = Result<UpdateClientCommandResponse>.Success(response);
        _clientServiceMock.Setup(s => s.UpdateClientAsync(command))
            .ReturnsAsync(successResult);

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
        var command = new UpdateClientCommand();
        _clientServiceMock.Setup(s => s.UpdateClientAsync(command))
            .ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("An error occurred while processing the request: Unexpected", result.Error);
    }
}
