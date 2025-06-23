using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;

public class CreateDietPlanCommand : IRequest<Result<CreateDietPlanCommandResponse>>
{
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double InitialWeight { get; set; }
    public double TargetWeight { get; set; }
    public Guid ClientId { get; set; }
    public Guid DietitianId { get; set; }
}