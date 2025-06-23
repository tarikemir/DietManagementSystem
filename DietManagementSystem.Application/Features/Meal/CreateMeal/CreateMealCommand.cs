using MediatR;

namespace DietManagementSystem.Application.Features.Meal.CreateMeal;

public class CreateMealCommand : IRequest<Result<CreateMealCommandResponse>>
{
    public string Title { get; set; } = null!;
    public DateOnly? ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Contents { get; set; } = null!;
    public Guid DietPlanId { get; set; }
}