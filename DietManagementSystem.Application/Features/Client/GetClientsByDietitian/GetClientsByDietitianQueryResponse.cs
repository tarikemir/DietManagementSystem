namespace DietManagementSystem.Application.Features.Client.GetClientsByDietitian;

public class GetClientsByDietitianQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double InitialWeight { get; set; }
    public string DietitianName { get; set; } = null!;
    public Guid DietitianId { get; set; }
    public Guid ApplicationUserId { get; set; }
}
