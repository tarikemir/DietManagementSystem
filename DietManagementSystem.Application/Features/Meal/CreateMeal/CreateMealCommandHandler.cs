using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Meal.CreateMeal;

public class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, Result<CreateMealCommandResponse>>
{
    private readonly IDietPlanService _dietPlanService;

    public CreateMealCommandHandler(IDietPlanService dietPlanService)
    {
        _dietPlanService = dietPlanService;
    }

    public async Task<Result<CreateMealCommandResponse>> Handle(CreateMealCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dietPlanService.AddMealToDietPlanAsync(request);
            if (result.IsFailure)
            {
                return Result<CreateMealCommandResponse>.Failure(result.Error);
            }
            return Result<CreateMealCommandResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<CreateMealCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
