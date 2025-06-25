namespace DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;

public class UpdateDietitianCommandResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
}
