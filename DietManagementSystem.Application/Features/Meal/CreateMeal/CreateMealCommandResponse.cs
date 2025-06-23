namespace DietManagementSystem.Application.Features.Meal.CreateMeal;

public class CreateMealCommandResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateOnly? ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Contents { get; set; } = null!;
}