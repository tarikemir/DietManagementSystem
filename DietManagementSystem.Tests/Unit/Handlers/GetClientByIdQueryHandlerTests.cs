using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Client.GetClient;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class GetClientByIdQueryHandlerTests
{
    private readonly Mock<IClientService> _clientServiceMock = new();

    private GetClientByIdQueryHandler CreateHandler() =>
        new(_clientServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenClientIsNull()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new GetClientByIdQuery { ClientId = Guid.NewGuid() };
        _clientServiceMock.Setup(s => s.GetClientAsync(command.ClientId))
            .ReturnsAsync((Result<GetClientByIdQueryResponse>)null!);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Client not found.", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenClientIsFound()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new GetClientByIdQuery { ClientId = Guid.NewGuid() };
        var clientResponse = new GetClientByIdQueryResponse
        {
            Id = command.ClientId,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            DietitianName = "Dietitian",
            InitialWeight = 60,
            DietitianId = Guid.NewGuid(),
            DietPlans = []
        };
        var successResult = Result<GetClientByIdQueryResponse>.Success(clientResponse);
        _clientServiceMock.Setup(s => s.GetClientAsync(command.ClientId))
            .ReturnsAsync(successResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(clientResponse, result.Value);
    }
}
