namespace DietManagementSystem.Application.Features.Dietitian.DeleteDietitian;

public class DeleteDietitianCommandResponse
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Message { get; set; } = null!;
}
