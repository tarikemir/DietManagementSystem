namespace DietManagementSystem.Application.Features.Client.GetClient;

public class GetClientByIdQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string DietitianName { get; set; } = null!;
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }
    public Guid ApplicationUserId { get; set; }
}
