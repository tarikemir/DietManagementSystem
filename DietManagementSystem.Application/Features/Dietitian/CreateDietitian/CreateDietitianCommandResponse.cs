namespace DietManagementSystem.Application.Features.Dietitian.CreateDietitian;

public class CreateDietitianCommandResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Guid ApplicationUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
