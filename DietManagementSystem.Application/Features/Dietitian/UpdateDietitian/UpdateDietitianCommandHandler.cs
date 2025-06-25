using DietManagementSystem.Application.Common;
using MediatR;

namespace DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;

public class UpdateDietitianCommandHandler
    : IRequestHandler<UpdateDietitianCommand, Result<UpdateDietitianCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDietitianCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateDietitianCommandResponse>> Handle(
        UpdateDietitianCommand request,
        CancellationToken cancellationToken)
    {
        var dietitian = await _unitOfWork.Dietitians.FirstOrDefaultAsync(d => d.Id == request.Id);
        if (dietitian == null)
            return Result<UpdateDietitianCommandResponse>.Failure("Dietitian not found");

        dietitian.FirstName = request.FirstName;
        dietitian.LastName = request.LastName;
        dietitian.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return Result<UpdateDietitianCommandResponse>.Success(new UpdateDietitianCommandResponse
        {
            Id = dietitian.Id,
            FirstName = dietitian.FirstName,
            LastName = dietitian.LastName,
            UpdatedAt = dietitian.UpdatedAt ?? DateTime.UtcNow
        });
    }
}
