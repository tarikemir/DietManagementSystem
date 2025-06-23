using DietManagementSystem.Application.Common;
using MediatR;

namespace DietManagementSystem.Application.Features.Dietitian.CreateDietitian;

public class CreateDietitianCommandHandler
    : IRequestHandler<CreateDietitianCommand, Result<CreateDietitianCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDietitianCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateDietitianCommandResponse>> Handle(
        CreateDietitianCommand request,
        CancellationToken cancellationToken)
    {
        var dietitian = new Domain.Entities.Dietitian
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            ApplicationUserId = request.ApplicationUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Dietitians.AddAsync(dietitian);
        await _unitOfWork.SaveChangesAsync();

        return Result<CreateDietitianCommandResponse>.Success(new CreateDietitianCommandResponse
        {
            Id = dietitian.Id,
            FirstName = dietitian.FirstName,
            LastName = dietitian.LastName,
            ApplicationUserId = dietitian.ApplicationUserId,
            CreatedAt = dietitian.CreatedAt
        });
    }
}
