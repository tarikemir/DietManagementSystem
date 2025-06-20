using DietManagementSystem.Domain.Entities.Common;
namespace DietManagementSystem.Domain.Entities;

public class Client : BaseEntity<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public double InitialWeight { get; set; }

    public Guid DietitianId { get; set; }
    public Dietitian Dietitian { get; set; } = null!;
    public ICollection<DietPlan> DietPlans { get; set; } = new List<DietPlan>();
}
