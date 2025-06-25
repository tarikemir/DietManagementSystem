using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Features.Meal.CreateMeal;
using DietManagementSystem.Application.Services;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class CreateMealCommandHandlerTests
{
    private readonly Mock<IDietPlanService> _dietPlanServiceMock = new();

    private CreateMealCommandHandler CreateHandler() =>
        new(_dietPlanServiceMock.Object);

    [Fact]
    public async Task Handle_ReturnsFailure_WhenAddMealFails()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateMealCommand();
        var failureResult = Result<CreateMealCommandResponse>.Failure("Add meal failed");
        _dietPlanServiceMock.Setup(s => s.AddMealToDietPlanAsync(command))
            .ReturnsAsync(failureResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Add meal failed", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenAddMealSucceeds()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new CreateMealCommand();
        var response = new CreateMealCommandResponse
        {
            Id = Guid.NewGuid(),
            Title = "Lunch",
            Contents = "Chicken and rice"
        };
        var successResult = Result<CreateMealCommandResponse>.Success(response);
        _dietPlanServiceMock.Setup(s => s.AddMealToDietPlanAsync(command))
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
        var command = new CreateMealCommand();
        _dietPlanServiceMock.Setup(s => s.AddMealToDietPlanAsync(command))
            .ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("An error occurred while processing the request: Unexpected", result.Error);
    }
}

