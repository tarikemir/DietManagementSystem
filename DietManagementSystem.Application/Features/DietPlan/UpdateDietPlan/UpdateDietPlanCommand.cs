using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.UpdateDietPlan;

public class UpdateDietPlanCommand : IRequest<Result<UpdateDietPlanCommandResponse>>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double InitialWeight { get; set; }
    public double TargetWeight { get; set; }
    public Guid ClientId { get; set; }
    public Guid DietitianId { get; set; }
}
