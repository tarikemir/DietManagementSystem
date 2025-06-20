using DietManagementSystem.Domain.Entities.Common;
using DietManagementSystem.Domain.Identity;

namespace DietManagementSystem.Domain.Entities;

public class Dietitian : BaseEntity<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<DietPlan> DietPlans { get; set; } = new List<DietPlan>();

    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;
}
