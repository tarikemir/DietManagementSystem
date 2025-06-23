using AutoMapper;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;

public class CreateDietPlanCommandHandler : IRequestHandler<CreateDietPlanCommand, Result<CreateDietPlanCommandResponse>>
{
    private readonly IDietPlanService _dietPlanService;

    public CreateDietPlanCommandHandler(IDietPlanService dietPlanService)
    {
        _dietPlanService = dietPlanService;
    }

    public async Task<Result<CreateDietPlanCommandResponse>> Handle(CreateDietPlanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dietPlanService.CreateDietPlanAsync(request);
            if (result.IsFailure)
            {
                return Result<CreateDietPlanCommandResponse>.Failure(result.Error);
            }
            return Result<CreateDietPlanCommandResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<CreateDietPlanCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
