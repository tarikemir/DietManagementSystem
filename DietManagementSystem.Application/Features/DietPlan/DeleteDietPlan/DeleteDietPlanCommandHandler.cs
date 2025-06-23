using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.DeleteDietPlan;

public class DeleteDietPlanCommandHandler : IRequestHandler<DeleteDietPlanCommand, Result<DeleteDietPlanCommandResponse>>
{
    private readonly IDietPlanService _dietPlanService;

    public DeleteDietPlanCommandHandler(IDietPlanService dietPlanService)
    {
        _dietPlanService = dietPlanService;
    }

    public async Task<Result<DeleteDietPlanCommandResponse>> Handle(DeleteDietPlanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dietPlanService.DeleteDietPlanAsync(request.Id);
            if (result.IsFailure)
            {
                return Result<DeleteDietPlanCommandResponse>.Failure(result.Error);
            }
            return Result<DeleteDietPlanCommandResponse>.Success(null); // Will be revisited...
        }
        catch (Exception e)
        {
            return Result<DeleteDietPlanCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
