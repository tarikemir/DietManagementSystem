using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.GetDietPlan;

public class GetDietPlanQueryHandler : IRequestHandler<GetDietPlanQuery, Result<GetDietPlanQueryResponse>>
{
    private readonly IDietPlanService _dietPlanService;

    public GetDietPlanQueryHandler(IDietPlanService dietPlanService)
    {
        _dietPlanService = dietPlanService;
    }

    public async Task<Result<GetDietPlanQueryResponse>> Handle(GetDietPlanQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dietPlanService.GetDietPlanAsync(request.Id);
            if (result.IsFailure)
            {
                return Result<GetDietPlanQueryResponse>.Failure(result.Error);
            }
            return Result<GetDietPlanQueryResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<GetDietPlanQueryResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
