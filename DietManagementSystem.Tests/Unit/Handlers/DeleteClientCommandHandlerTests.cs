using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Client.DeleteClient;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class DeleteClientCommandHandlerTests
{
    private readonly Mock<IClientService> _clientServiceMock = new();

    private DeleteClientCommandHandler CreateHandler() =>
        new(_clientServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenDeleteClientFails()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteClientCommand { ClientId = Guid.NewGuid() };
        var failureResult = Result.Failure("Delete failed");
        _clientServiceMock.Setup(s => s.DeleteClientAsync(command.ClientId))
            .ReturnsAsync(failureResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Delete failed", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenDeleteClientSucceeds()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteClientCommand { ClientId = Guid.NewGuid() };
        var successResult = Result.Success();
        _clientServiceMock.Setup(s => s.DeleteClientAsync(command.ClientId))
            .ReturnsAsync(successResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteClientCommand { ClientId = Guid.NewGuid() };
        _clientServiceMock.Setup(s => s.DeleteClientAsync(command.ClientId))
            .ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("An error occurred while processing the request: Unexpected", result.Error);
    }
}
