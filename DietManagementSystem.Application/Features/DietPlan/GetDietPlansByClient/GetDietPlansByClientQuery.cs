using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.GetDietPlansByClient;

public class GetDietPlansByClientQuery : IRequest<Result<List<GetDietPlansByClientQueryResponse>>>
{
    public Guid ClientId { get; set; }
}