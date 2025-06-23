namespace DietManagementSystem.Application.Features.Client.UpdateClient;

public class UpdateClientCommandResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }
    public Guid ApplicationUserId { get; set; }
}
