using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.GetDietPlan;

public class GetDietPlanQuery : IRequest<Result<GetDietPlanQueryResponse>>
{
    public Guid Id;
}
