using System;
using System.Threading;
using System.Threading.Tasks;
using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;
using DietManagementSystem.Domain.Entities;
using Moq;
using Xunit;

namespace DietManagementSystem.Tests.Unit.Handlers;
public class UpdateDietitianCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IRepository<Dietitian>> _dietitianRepoMock = new();

    private UpdateDietitianCommandHandler CreateHandler()
    {
        _unitOfWorkMock.Setup(u => u.Dietitians).Returns(_dietitianRepoMock.Object);
        return new UpdateDietitianCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenDietitianNotFound()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new UpdateDietitianCommand { Id = Guid.NewGuid(), FirstName = "New", LastName = "Name" };
        _dietitianRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Dietitian, bool>>>()))
            .ReturnsAsync((Dietitian)null!);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Dietitian not found", result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenDietitianUpdated()
    {
        // Arrange
        var handler = CreateHandler();
        var id = Guid.NewGuid();
        var oldDietitian = new Dietitian
        {
            Id = id,
            FirstName = "Old",
            LastName = "Name",
            UpdatedAt = null
        };
        _dietitianRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Dietitian, bool>>>()))
            .ReturnsAsync(oldDietitian);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var command = new UpdateDietitianCommand { Id = id, FirstName = "New", LastName = "Name" };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal("New", result.Value.FirstName);
        Assert.Equal("Name", result.Value.LastName);
        Assert.True(result.Value.UpdatedAt <= DateTime.UtcNow && result.Value.UpdatedAt > DateTime.UtcNow.AddMinutes(-1));
    }
}

