using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.DeleteDietPlan;

public class DeleteDietPlanCommand : IRequest<Result<DeleteDietPlanCommandResponse>>
{
    public Guid Id;
}
