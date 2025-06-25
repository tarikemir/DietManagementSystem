using DietManagementSystem.Application.Features.DietPlan.GetDietPlan;

namespace DietManagementSystem.Application.Features.Client.GetClients;

public class GetClientsQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; } = null!;
    public double InitialWeight { get; set; }
    public string DietitianName { get; set; } = null!;
    public Guid DietitianId { get; set; }
    public Guid ApplicationUserId { get; set; }

}
