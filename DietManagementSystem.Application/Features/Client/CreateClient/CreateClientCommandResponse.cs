namespace DietManagementSystem.Application.Features.Client.CreateClient;

public class CreateClientCommandResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }
}