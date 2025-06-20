using DietManagementSystem.Domain.Entities.Common;

namespace DietManagementSystem.Domain.Entities;

public class DietPlan : BaseEntity<Guid>
{
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double InitialWeight { get; set; }
    public double TargetWeight { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public Guid DietitianId { get; set; }
    public Dietitian Dietitian { get; set; } = null!;
    public ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
