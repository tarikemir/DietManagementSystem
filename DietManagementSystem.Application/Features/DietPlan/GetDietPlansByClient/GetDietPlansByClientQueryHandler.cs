using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.GetDietPlansByClient;

public class GetDietPlansByClientQueryHandler : IRequestHandler<GetDietPlansByClientQuery, Result<List<GetDietPlansByClientQueryResponse>>>
{
    private readonly IDietPlanService _dietPlanService;

    public GetDietPlansByClientQueryHandler(IDietPlanService dietPlanService)
    {
        _dietPlanService = dietPlanService;
    }

    public async Task<Result<List<GetDietPlansByClientQueryResponse>>> Handle(GetDietPlansByClientQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dietPlanService.GetDietPlansByClientAsync(request.ClientId);
            if (result.IsFailure)
            {
                return Result<List<GetDietPlansByClientQueryResponse>>.Failure(result.Error ?? "Unknown error occurred.");
            }

            var mappedResult = result.Value.Select(dietPlan => new GetDietPlansByClientQueryResponse
            {
                Id = dietPlan.Id,
                Title = dietPlan.Title,
                StartDate = dietPlan.StartDate,
                EndDate = dietPlan.EndDate,
                InitialWeight = dietPlan.InitialWeight,
                TargetWeight = dietPlan.TargetWeight,
                ClientId = dietPlan.ClientId,
                ClientName = dietPlan.ClientName,
                DietitianId = dietPlan.DietitianId,
                DietitianName = dietPlan.DietitianName
            }).ToList();

            return Result<List<GetDietPlansByClientQueryResponse>>.Success(mappedResult);
        }
        catch (Exception e)
        {
            return Result<List<GetDietPlansByClientQueryResponse>>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
