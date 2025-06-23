using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.UpdateDietPlan;

public class UpdateDietPlanCommandHandler : IRequestHandler<UpdateDietPlanCommand, Result<UpdateDietPlanCommandResponse>>
{
    private readonly IDietPlanService _dietPlanService;

    public UpdateDietPlanCommandHandler(IDietPlanService dietPlanService)
    {
        _dietPlanService = dietPlanService;
    }

    public async Task<Result<UpdateDietPlanCommandResponse>> Handle(UpdateDietPlanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dietPlanService.UpdateDietPlanAsync(request);
            if (result.IsFailure)
            {
                return Result<UpdateDietPlanCommandResponse>.Failure(result.Error);
            }
            return Result<UpdateDietPlanCommandResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<UpdateDietPlanCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
