using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Features.Meal.CreateMeal;

namespace DietManagementSystem.Application.Features.DietPlan.GetDietPlan;

public class GetDietPlanQueryResponse : IDietPlan
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double InitialWeight { get; set; }
    public double TargetWeight { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = null!;
    public Guid DietitianId { get; set; }
    public string DietitianName { get; set; } = null!;
    public List<CreateMealCommandResponse> Meals { get; set; } = new();
}
