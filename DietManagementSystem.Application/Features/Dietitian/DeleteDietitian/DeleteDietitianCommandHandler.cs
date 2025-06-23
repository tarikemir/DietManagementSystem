using DietManagementSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.Dietitian.DeleteDietitian;

public class DeleteDietitianCommandHandler
    : IRequestHandler<DeleteDietitianCommand, Result<DeleteDietitianCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDietitianCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeleteDietitianCommandResponse>> Handle(
        DeleteDietitianCommand request,
        CancellationToken cancellationToken)
    {
        var dietitian = await _unitOfWork.Dietitians
            .Query()
            .Include(d => d.Clients)
            .Include(d => d.DietPlans)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (dietitian == null)
            return Result<DeleteDietitianCommandResponse>.Failure("Dietitian not found");

        await _unitOfWork.Dietitians.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return Result<DeleteDietitianCommandResponse>.Success(new DeleteDietitianCommandResponse
        {
            Id = request.Id,
            IsDeleted = true,
            Message = "Dietitian deleted successfully"
        });
    }
}
