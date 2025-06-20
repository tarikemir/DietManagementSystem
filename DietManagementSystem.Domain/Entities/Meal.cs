using DietManagementSystem.Domain.Entities.Common;

namespace DietManagementSystem.Domain.Entities;

public class Meal : BaseEntity<Guid>
{
    public string Title { get; set; } = null!;
    public DateOnly? ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Contents { get; set; } = null!;


    public Guid DietPlanId { get; set; }
    public DietPlan DietPlan { get; set; } = null!;
}
