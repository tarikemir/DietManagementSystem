using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Client.GetClientsByDietitian;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class GetClientsByDietitianQueryHandlerTests
{
    private readonly Mock<IClientService> _clientServiceMock = new();

    private GetClientsByDietitianQueryHandler CreateHandler() =>
        new(_clientServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenResultIsNull()
    {
        // Arrange
        var handler = CreateHandler();
        var query = new GetClientsByDietitianQuery { DietitianId = Guid.NewGuid() };
        _clientServiceMock.Setup(s => s.GetClientsByDietitianAsync(query.DietitianId))
            .ReturnsAsync((Result<List<GetClientsByDietitianQueryResponse>>)null!);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Clients not found.", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenClientsFound()
    {
        // Arrange
        var handler = CreateHandler();
        var query = new GetClientsByDietitianQuery { DietitianId = Guid.NewGuid() };
        var clients = new List<GetClientsByDietitianQueryResponse>
        {
            new GetClientsByDietitianQueryResponse
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                InitialWeight = 65,
                DietitianName = "Dr. Diet",
                DietitianId = query.DietitianId,
                ApplicationUserId = Guid.NewGuid()
            }
        };
        var successResult = Result<List<GetClientsByDietitianQueryResponse>>.Success(clients);
        _clientServiceMock.Setup(s => s.GetClientsByDietitianAsync(query.DietitianId))
            .ReturnsAsync(successResult);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        Assert.Equal("Alice", result.Value[0].FirstName);
    }
}
