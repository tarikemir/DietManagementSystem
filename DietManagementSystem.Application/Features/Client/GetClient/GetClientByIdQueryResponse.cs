using DietManagementSystem.Application.Features.DietPlan.GetDietPlan;

namespace DietManagementSystem.Application.Features.Client.GetClient;

public class GetClientByIdQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DietitianName { get; set; } = null!;
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }

    public List<GetDietPlanQueryResponse> DietPlans { get; set; }
}
